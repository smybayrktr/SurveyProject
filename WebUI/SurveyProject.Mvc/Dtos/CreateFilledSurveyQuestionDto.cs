namespace SurveyProject.Mvc.Dtos;

public class CreateFilledSurveyQuestionDto
{
    public string Question { get; set; }
    public int QuestionType { get; set; }
    public List<CreateFilledSurveyQuestionOptionDto>? SurveyQuestionOptions { get; set; }
}