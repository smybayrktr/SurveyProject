using SurveyProject.Entities.Enums;

namespace SurveyProject.Entities;

public class SurveyQuestion : IEntity
{
    public int Id { get; set; }
    public int SurveyId { get; set; }
    public string Question { get; set; }
    public QuestionType QuestionType { get; set; }
}