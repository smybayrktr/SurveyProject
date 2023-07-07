using SurveyProject.Entities.Enums;

namespace SurveyProject.DataTransferObjects.Requests;

public class CreateSurveyQuestionRequest
{
	public int SurveyId { get; set; }
	public string Question { get; set; }
	public QuestionType QuestionType { get; set; }
}