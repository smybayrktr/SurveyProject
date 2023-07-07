using Microsoft.AspNetCore.Mvc;
using SurveyProject.Mvc.Helpers.CookieHelper;

namespace SurveyProject.Mvc.ViewComponents;

public class HeaderMenuItems : ViewComponent
{
    private readonly ICookieHelper _cookieHelper;

    public HeaderMenuItems(ICookieHelper cookieHelper)
    {
        _cookieHelper = cookieHelper;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var checkJwt = String.IsNullOrEmpty(_cookieHelper.GetJwtFromCookie());
        return View(checkJwt);
    }

}