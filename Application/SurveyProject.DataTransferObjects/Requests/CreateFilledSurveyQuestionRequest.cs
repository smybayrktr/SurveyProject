using SurveyProject.Entities.Enums;

namespace SurveyProject.DataTransferObjects.Requests;

public class CreateFilledSurveyQuestionRequest
{
	public string Question { get; set; }
	public QuestionType QuestionType { get; set; }
	public List<CreateFilledSurveyQuestionOptionRequest>? SurveyQuestionOptions { get; set; }
}