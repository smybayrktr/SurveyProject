namespace SurveyProject.Core.Constants;

public static class EmailMessages
{
    public const string RegisterTitle = "You are successfuly registered Survey App";
    public const string RegisterSubject = "You are successfuly registered Survey App";
    public const string SurveyAnswerCreatedTitle = "New answer to your survey";
    public const string SurveyAnswerCreatedSubject = "New answer to your survey";   
    public const string SurveyCreatedTitle = "Your survey has been created";
    public const string SurveyCreatedSubject = "Your survey has been created";

    public static string GetRegisterBody(string name)
    {
        return $"<p>Hello {name}</p><br><p>Welcome to the Survey App</p>";
    }

    public static string GetRegisterBodyWithPassword(string name, string lastName, string password)
    {
        return $"<p>Hello {name} {lastName}</p><br><p>Welcome to the Survey App</p><br><p>Your password to login is {password}</p>";
    }
    
    public static string GetSurveyAnswerCreatedBody(string name, string lastName)
    {
        return $"<p>Hello {name} {lastName}</p><br><p>Your survey has been answered.</p>";
    }
    
    public static string GetSurveyCreatedBody(string name, string lastName, string url)
    {
        return $"<p>Hello {name} {lastName}</p><br><p>Your survey has been created, you can share via {url}</p>";
    }
}