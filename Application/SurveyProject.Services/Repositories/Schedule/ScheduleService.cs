using Hangfire;
using SurveyProject.Services.Repositories.Email;

namespace SurveyProject.Services.Repositories.Schedule;

public class ScheduleService
{
    public static void ScheduleSendRegisterEmailWithPassword(string name, string lastName, string email,  string password)
    {
        BackgroundJob.Schedule<IEmailService>(x => x.SendRegisterEmailWithPassword(name, lastName, email, password), TimeSpan.FromMinutes(1));
    }
    
    public static void SendSurveyAnswerCreatedEmail(string name, string lastName, string email)
    {
        BackgroundJob.Enqueue<IEmailService>(x => x.SendSurveyAnswerCreatedEmail(name, lastName, email));
    }
    
    public static void SendSurveyCreatedEmail(string name, string lastName, string url, string email)
    {
        BackgroundJob.Enqueue<IEmailService>(x => x.SendSurveyCreatedEmail(name, lastName, url, email));
    }
}