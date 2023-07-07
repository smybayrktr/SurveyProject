namespace SurveyProject.DataTransferObjects.Requests;

public class CreateSurveyQuestionOptionRequest
{
	public int SurveyQuestionId { get; set; }
	public string Text { get; set; }
}