using Microsoft.AspNetCore.Mvc;
using SurveyProject.Mvc.Models;

namespace SurveyProject.Mvc.ViewComponents;

public class SurveyQuestion : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(int count)
    {
        var createSurveyQuestionViewModel = new SurveyQuestionViewModel
        {
            Count = count
        };
        return View(createSurveyQuestionViewModel);
    }
}