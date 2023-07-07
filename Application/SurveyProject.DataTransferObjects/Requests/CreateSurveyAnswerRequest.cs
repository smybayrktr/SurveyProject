namespace SurveyProject.DataTransferObjects.Requests;

public class CreateSurveyAnswerRequest
{
	public int SurveyQuestionId { get; set; }
	public string? SingleLinePlainTextAnswer { get; set; }
	public string? MultipleLinePlainTextAnswer { get; set; }
	public int? ScoringAnswer { get; set; }
	public int? MultipleChoiceAnswer { get; set; }
}