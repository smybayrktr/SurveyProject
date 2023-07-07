namespace SurveyProject.Mvc.Dtos;

public class GetSurveyByUrlDto
{
    public GetSurveyByUrlDto()
    {
        SurveyQuestions = new List<GetSurveyQuestionByUrlDto>();
    }

    public List<GetSurveyQuestionByUrlDto> SurveyQuestions { get; set; }
}