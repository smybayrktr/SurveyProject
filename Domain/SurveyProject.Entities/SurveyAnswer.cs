using SurveyProject.Core.Helpers.DateHelper;

namespace SurveyProject.Entities;

public class SurveyAnswer: IEntity
{
	public int Id { get; set; }
	public int SurveyQuestionId { get; set; }
	public string? SingleLinePlainTextAnswer { get; set; }
	public string? MultipleLinePlainTextAnswer { get; set; }
	public int? ScoringAnswer { get; set; }
	public int? MultipleChoiceAnswer { get; set; }
	public long CreatedAt { get; set; } = DateHelper.DateToTimestampt(DateTime.Now);
}