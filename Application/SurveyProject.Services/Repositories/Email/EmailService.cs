using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using SurveyProject.Core.Constants;
using SurveyProject.Core.Helpers.EmailHelper;
using SurveyProject.Core.Utilities.Results;

namespace SurveyProject.Services.Repositories.Email;

public class EmailService : IEmailService
{
    private readonly EmailConfiguration _emailConfig;

    public EmailService(EmailConfiguration emailConfig)
    {
        _emailConfig = emailConfig;
    }

    public async Task<IResult> SendRegisterEmailWithPassword(string name, string lastName, string email,
        string password)
    {
        await SendEmail(new EmailMessage(new[] { email }, EmailMessages.RegisterTitle, EmailMessages.RegisterSubject,
            EmailMessages.GetRegisterBodyWithPassword(name, lastName, password)));
        return new SuccessResult(ApiStatusCodes.Ok);
    }

    public async Task<IResult> SendSurveyAnswerCreatedEmail(string name, string lastname, string email)
    {
        await SendEmail(new EmailMessage(new[] { email }, EmailMessages.SurveyAnswerCreatedTitle,
            EmailMessages.SurveyAnswerCreatedSubject,
            EmailMessages.GetSurveyAnswerCreatedBody(name, lastname)));
        return new SuccessResult(ApiStatusCodes.Ok);
    }
    
    public async Task<IResult> SendSurveyCreatedEmail(string name, string lastname, string url, string email)
    {
        await SendEmail(new EmailMessage(new[] { email }, EmailMessages.SurveyCreatedTitle,
            EmailMessages.SurveyCreatedSubject,
            EmailMessages.GetSurveyCreatedBody(name, lastname, url)));
        return new SuccessResult(ApiStatusCodes.Ok);
    }

    private async Task<IResult> SendEmail(EmailMessage message)
    {
        var emailMessage = CreateEmailMessage(message);
        await Send(emailMessage);
        return new SuccessResult(ApiStatusCodes.Ok);
    }

    private MimeMessage CreateEmailMessage(EmailMessage message)
    {
        var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(message.Title, _emailConfig.From));
        emailMessage.To.AddRange(message.To);
        emailMessage.Subject = message.Subject;
        var builder = new BodyBuilder();
        builder.HtmlBody = message.Content;
        emailMessage.Body = builder.ToMessageBody();
        return emailMessage;
    }

    private async Task Send(MimeMessage mailMessage)
    {
        using (var client = new SmtpClient())
        {
            try
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client
                    .ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.SslOnConnect)
                    .ConfigureAwait(false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password).ConfigureAwait(false);
                await client.SendAsync(mailMessage).ConfigureAwait(false);
            }
            finally
            {
                await client.DisconnectAsync(true).ConfigureAwait(false);
                client.Dispose();
            }
        }
    }
}