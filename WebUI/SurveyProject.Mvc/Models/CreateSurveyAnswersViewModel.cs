namespace SurveyProject.Mvc.Models;

public class CreateSurveyAnswersViewModel
{
    public CreateSurveyAnswersViewModel()
    {
        CreateSurveyAnswerRequests = new List<CreateSurveyAnswerViewModel>();
    }

    public List<CreateSurveyAnswerViewModel> CreateSurveyAnswerRequests { get; set; }
}