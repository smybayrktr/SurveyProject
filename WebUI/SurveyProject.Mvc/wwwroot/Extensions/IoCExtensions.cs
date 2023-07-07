using System;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using SurveyProject.Core.Helpers.EmailHelper;
using SurveyProject.Core.Helpers.FileHelper;
using SurveyProject.Core.Helpers.UrlHelper;
using SurveyProject.Infrastructure.Data;
using SurveyProject.Infrastructure.Repositories;
using SurveyProject.Infrastructure.Repositories.EntityFramework;
using SurveyProject.Services.Repositories.AppUser;
using SurveyProject.Services.Repositories.Auth;
using SurveyProject.Services.Repositories.Email;
using SurveyProject.Services.Repositories.Survey;
using SurveyProject.Services.Repositories.SurveyAnswer;
using SurveyProject.Services.Repositories.SurveyQuestion;
using SurveyProject.Services.Repositories.SurveyQuestionChoice;

namespace SurveyProject.Mvc.Extensions
{
    public static class IoCExtensions
    {
        public static IServiceCollection AddInjections(this IServiceCollection services, IConfiguration configuration)
        {
            //uygulama build olmadan önce applicationun kullanacağı nesneleri containere eklememiz lazım.
            services.AddScoped<ISurveyRespository, EfSurveyRepository>();
            services.AddScoped<ISurveyService, SurveyService>();

            services.AddScoped<ISurveyAnswerRepository, EfSurveyAnswerRepository>();
            services.AddScoped<ISurveyAnswerService, SurveyAnswerService>();

            services.AddScoped<ISurveyQuestionChoiceRepository, EfSurveyQuestionChoiceRepository>();
            services.AddScoped<ISurveyQuestionChoiceService, SurveyQuestionChoiceService>();

            services.AddScoped<ISurveyQuestionRepository, EfSurveyQuestionRepository>();
            services.AddScoped<ISurveyQuestionService, SurveyQuestionService>();

            services.AddScoped<IUserService, UserService>();


            services.AddScoped<IFileHelper, FileHelper>();
            services.AddScoped<IUrlHelper, UrlHelper>();
            services.AddScoped<IEmailService, EmailService>();


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

            var emailConfig = configuration.GetSection("EmailConfiguration")
                                           .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            return services;

        }
    }
}

