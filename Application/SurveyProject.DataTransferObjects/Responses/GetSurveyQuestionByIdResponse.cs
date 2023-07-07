using SurveyProject.Entities.Enums;

namespace SurveyProject.DataTransferObjects.Responses;

public class GetSurveyQuestionByIdResponse
{
    public int Id { get; set; }
    public string Question { get; set; }
    public QuestionType QuestionType { get; set; }
}