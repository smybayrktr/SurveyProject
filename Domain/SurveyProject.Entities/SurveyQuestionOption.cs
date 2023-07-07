namespace SurveyProject.Entities;

public class SurveyQuestionOption : IEntity
{
    public int Id { get; set; }
    public int SurveyQuestionId { get; set; }
    public string Text { get; set; }
}