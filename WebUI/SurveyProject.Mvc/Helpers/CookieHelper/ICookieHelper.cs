namespace SurveyProject.Mvc.Helpers.CookieHelper;

public interface ICookieHelper
{
	string? GetJwtFromCookie();
	void SetJwtCookie(string token);
	void DeleteJwtCookie();
}