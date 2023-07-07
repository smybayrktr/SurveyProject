using SurveyProject.WebApi.Middlewares;

namespace SurveyProject.WebApi.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void UseExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}