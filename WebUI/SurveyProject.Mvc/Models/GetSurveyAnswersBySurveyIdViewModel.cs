namespace SurveyProject.Mvc.Models;

public class GetSurveyAnswersBySurveyIdViewModel
{
    public GetSurveyAnswersBySurveyIdViewModel()
    {
        SurveyAnswers = new List<GetSurveyAnswerBySurveyIdViewModel>();
    }

    public List<GetSurveyAnswerBySurveyIdViewModel> SurveyAnswers { get; set; }
}