using Microsoft.AspNetCore.Mvc;
using SurveyProject.Mvc.Models;

namespace SurveyProject.Mvc.ViewComponents;

public class SurveyQuestionOption : ViewComponent
{

    public async Task<IViewComponentResult> InvokeAsync(int questionType, int count, bool renderFirstTime=true)
    {
        var createSurveyQuestionViewModel = new SurveyQuestionOptionViewModel {
            QuestionType = questionType,
            Count = count,
            RenderFirstTime = renderFirstTime
        };
        return View(createSurveyQuestionViewModel);
    }

}