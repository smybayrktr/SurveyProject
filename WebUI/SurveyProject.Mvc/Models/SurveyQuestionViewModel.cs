namespace SurveyProject.Mvc.Models;

public class SurveyQuestionViewModel
{
	public int Id { get; set; }
	public int? Count { get; set; }
	public string Question { get; set; }
	public int QuestionType { get; set; }
	public List<SurveyQuestionOptionViewModel>? SurveyQuestionOptions { get; set; }
}