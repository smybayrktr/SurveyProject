using System.Text;
using System.Text.Json;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SurveyProject.Core.Constants;
using SurveyProject.Core.Helpers.EmailHelper;
using SurveyProject.Core.Helpers.UrlHelper;
using SurveyProject.Core.Utilities.Jwt;
using SurveyProject.Core.Utilities.Results;
using SurveyProject.Entities;
using SurveyProject.Infrastructure.Data;
using SurveyProject.Infrastructure.Repositories;
using SurveyProject.Infrastructure.Repositories.EntityFramework;
using SurveyProject.Services.Mappings;
using SurveyProject.Services.Repositories.AppUser;
using SurveyProject.Services.Repositories.Auth;
using SurveyProject.Services.Repositories.Cache;
using SurveyProject.Services.Repositories.Email;
using SurveyProject.Services.Repositories.Survey;
using SurveyProject.Services.Repositories.SurveyAnswer;
using SurveyProject.Services.Repositories.SurveyQuestion;
using SurveyProject.Services.Repositories.SurveyQuestionOption;

namespace SurveyProject.WebApi.Extensions;

public static class IoCExtensions
{
    public static IServiceCollection AddInjections(this IServiceCollection services, IConfiguration configuration)
    {
        //uygulama build olmadan önce applicationun kullanacağı nesneleri containere eklememiz lazım.

        #region Dependency Injections
        
        services.AddScoped<ISurveyRespository, EfSurveyRepository>();
        services.AddScoped<ISurveyService, SurveyService>();

        services.AddScoped<ISurveyAnswerRepository, EfSurveyAnswerRepository>();
        services.AddScoped<ISurveyAnswerService, SurveyAnswerService>();

        services.AddScoped<ISurveyQuestionOptionRepository, EfSurveyQuestionOptionRepository>();
        services.AddScoped<ISurveyQuestionOptionService, SurveyQuestionOptionService>();

        services.AddScoped<ISurveyQuestionRepository, EfSurveyQuestionRepository>();
        services.AddScoped<ISurveyQuestionService, SurveyQuestionService>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IUrlHelper, UrlHelper>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddDbContext<SurveyContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Survey"))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking),
            ServiceLifetime.Transient);

        services.AddDbContext<HangfireContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Hangfire"))
                    .UseQueryTrackingBehavior(
                        QueryTrackingBehavior.NoTracking),
            ServiceLifetime.Transient);

        services.AddHttpContextAccessor();

        #endregion

        #region Authentication

        services.AddIdentity<User, IdentityRole<int>>(opt =>
            {
                opt.Password.RequiredLength = 8;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<SurveyContext>()
            .AddDefaultTokenProviders();

        var tokenOption = configuration.GetSection("TokenOption").Get<TokenOption>();

        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = tokenOption.Issuer,
                    ValidateIssuer = true,
                    ValidAudience = tokenOption.Audience,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOption.SecurityKey)),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.Response.OnStarting(async () =>
                        {
                            context.Response.StatusCode = ApiStatusCodes.Unauthorized;
                            var response = new ErrorResult(ApiMessages.Unauthorized, 401);
                            await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
                        });

                        return Task.CompletedTask;
                    }
                };
            });

        #endregion
        
        #region Hangfire

        services.AddHangfire(config =>
        {
            var option = new SqlServerStorageOptions
            {
                PrepareSchemaIfNecessary = true,
                QueuePollInterval = TimeSpan.FromMinutes(5),
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                UseRecommendedIsolationLevel = true,
                UsePageLocksOnDequeue = true,
                DisableGlobalLocks = true
            };
            config.UseSqlServerStorage(configuration.GetConnectionString("Hangfire"), option)
                .WithJobExpirationTimeout(TimeSpan.FromHours(6));
        });
        services.AddHangfireServer();
        GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 7 });

        #endregion

        #region Automapper

        services.AddAutoMapper(typeof(MapProfile));

        #endregion

        #region Email

        var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
        services.AddSingleton(emailConfig);

        #endregion

        #region Cache

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
        });

        #endregion
        
        #region Swagger

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

        #endregion
        
        return services;
    }
}