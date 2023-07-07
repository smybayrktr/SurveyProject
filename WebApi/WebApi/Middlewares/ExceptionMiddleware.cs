using System.Net;
using Newtonsoft.Json;
using SurveyProject.Core.Constants;
using SurveyProject.Core.Utilities.Results;

namespace SurveyProject.WebApi.Middlewares;

public class ExceptionMiddleware
{
    private RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(httpContext,e);
        }
    }

    private Task HandleExceptionAsync(HttpContext httpContext, Exception e)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = ApiStatusCodes.InternalServerError;

        return httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorResult(ApiMessages.InternalServerError, ApiStatusCodes.InternalServerError)));
    }
}