namespace SurveyProject.Mvc.Helpers.CookieHelper;

public class CookieHelper : ICookieHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CookieHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetJwtFromCookie()
    {
        return _httpContextAccessor.HttpContext.Request.Cookies["SurveyAuth"];
    }

    public void SetJwtCookie(string token)
    {
        _httpContextAccessor.HttpContext.Response.Cookies.Append("SurveyAuth", token, new CookieOptions
        {
            HttpOnly = true,
            Expires = DateTimeOffset.UtcNow.AddMinutes(1440)
        });
    }

    public void DeleteJwtCookie()
    {
        foreach (var cookie in _httpContextAccessor.HttpContext.Request.Cookies.Keys)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookie,
                new CookieOptions { Secure = true });
        }
    }
}