
namespace SurveyProject.Core.JoinDtos;

public class GetSurveyQuestionByUrlDto
{
    public int Id { get; set; }
    public string Question { get; set; }
    public int QuestionType { get; set; }
    public List<GetSurveyQuestionOptionByUrlDto>? SurveyQuestionOptions { get; set; }
}