namespace SurveyProject.Mvc.Models;

public class SurveyQuestionOptionViewModel
{
	public int Id { get; set; }
	public string Text { get; set; }
	public bool RenderFirstTime { get; set; }
	public int Count { get; set; }
	public int QuestionType { get; set; }
}