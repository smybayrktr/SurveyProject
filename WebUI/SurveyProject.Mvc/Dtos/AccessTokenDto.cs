namespace SurveyProject.Mvc.Dtos;

public class AccessTokenDto
{
	public string Token { get; set; }
	public DateTime Expiration { get; set; }
}