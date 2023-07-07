namespace SurveyProject.Core.Utilities.Jwt;

public class AccessToken
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}