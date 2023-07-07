using Hangfire;
using HangfireBasicAuthenticationFilter;
using SurveyProject.Infrastructure.Data;
using SurveyProject.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();

builder.Services.AddInjections(builder.Configuration);

var app = builder.Build();

#region CreateDatabases

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

var surveyContext = services.GetRequiredService<SurveyContext>();
surveyContext.Database.EnsureCreated();

var hangfireContext = services.GetRequiredService<HangfireContext>();
hangfireContext.Database.EnsureCreated();

#endregion

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionMiddleware();

app.UseCookiePolicy(new CookiePolicyOptions()
{
    MinimumSameSitePolicy = SameSiteMode.Lax
});
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Survey Hangfire",
    AppPath = "/hangfire",
    Authorization = new[]
    {
        new HangfireCustomBasicAuthenticationFilter
        {
            User = builder.Configuration.GetSection("HangfireSettings:User").Value,
            Pass = builder.Configuration.GetSection("HangfireSettings:Password").Value
        }
    },
    IgnoreAntiforgeryToken = true
});

app.MapControllers();

app.Run();