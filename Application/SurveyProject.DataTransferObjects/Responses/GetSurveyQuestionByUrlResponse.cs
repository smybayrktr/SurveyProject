using SurveyProject.Entities.Enums;

namespace SurveyProject.DataTransferObjects.Responses;

public class GetSurveyQuestionByUrlResponse
{
    public int Id { get; set; }
    public string Question { get; set; }
    public QuestionType QuestionType { get; set; }
    public List<GetSurveyQuestionOptionByUrlResponse>? SurveyQuestionOptions { get; set; }
}