namespace SurveyProject.Mvc.Dtos;

public class GetSurveyAnswerBySurveyIdDto
{
    public string Question { get; set; }
    public int QuestionType { get; set; }
    public List<GetSelectedSurveyAnswerDto> SelectedSurveyAnswers { get; set; }
    public int TotalAnswer { get; set; }
}