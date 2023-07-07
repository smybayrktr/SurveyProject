using SurveyProject.Core.Utilities.Results;

namespace SurveyProject.Services.Repositories.Email;

public interface IEmailService
{
    Task<IResult> SendRegisterEmailWithPassword(string name, string lastName, string email, string password);
    Task<IResult> SendSurveyAnswerCreatedEmail(string name, string lastname, string email);
    Task<IResult> SendSurveyCreatedEmail(string name, string lastname, string url, string email);
}