using Microsoft.AspNetCore.Authentication.Cookies;
using SurveyProject.Mvc.Client;
using SurveyProject.Mvc.Helpers.CookieHelper;
using SurveyProject.Mvc.Helpers.UrlHelper;
using SurveyProject.Mvc.Mappings;
using SurveyProject.Mvc.Utilities.Google;
using SurveyProject.Mvc.Utilities.IoC;

namespace SurveyProject.Mvc.Extensions;

public static class IoCExtensions
{
    public static IServiceCollection AddInjections(this IServiceCollection services, IConfiguration configuration)
    {
        #region Dependency Injections

        services.AddHttpContextAccessor();
        services.AddHttpClient<IHttpClientService, HttpClientService>();
        services.AddSingleton<ICookieHelper, CookieHelper>();
        services.AddSingleton<IUrlHelper, UrlHelper>();
        services.AddAutoMapper(typeof(MapProfile));
        ServiceTool.Create(services);

        #endregion

        #region Authentication

        services.Configure<CookieAuthenticationOptions>(options =>
        {
            options.Cookie.Name = "SurveyAuth";
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(1440);
            options.LoginPath = "/login";
        });

        var googleConfiguration = configuration.GetSection("Google").Get<GoogleConfiguration>();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie()
            .AddGoogle(options =>
            {
                options.ClientId = googleConfiguration.ClientId;
                options.ClientSecret = googleConfiguration.ClientSecret;
            });

        #endregion

        return services;
    }
}