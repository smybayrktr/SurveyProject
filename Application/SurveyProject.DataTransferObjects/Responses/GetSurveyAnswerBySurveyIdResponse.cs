using SurveyProject.Entities.Enums;

namespace SurveyProject.DataTransferObjects.Responses;

public class GetSurveyAnswerBySurveyIdResponse
{
    public string Question { get; set; }
    public QuestionType QuestionType { get; set; }
    public List<GetSelectedSurveyAnswerResponse> SelectedSurveyAnswers { get; set; }
    public int TotalAnswer { get; set; }
}