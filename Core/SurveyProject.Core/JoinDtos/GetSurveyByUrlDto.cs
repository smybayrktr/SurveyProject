namespace SurveyProject.Core.JoinDtos;

public class GetSurveyByUrlDto
{
    public GetSurveyByUrlDto()
    {
        SurveyQuestions = new List<GetSurveyQuestionByUrlDto>();
    }

    public List<GetSurveyQuestionByUrlDto> SurveyQuestions { get; set; }
}