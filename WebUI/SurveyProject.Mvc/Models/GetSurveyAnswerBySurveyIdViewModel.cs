namespace SurveyProject.Mvc.Models;

public class GetSurveyAnswerBySurveyIdViewModel
{
    public string Question { get; set; }
    public int QuestionType { get; set; }
    public List<GetSelectedSurveyAnswerViewModel> SelectedSurveyAnswers { get; set; }
    public int TotalAnswer { get; set; }
}