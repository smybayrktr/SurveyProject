namespace SurveyProject.Mvc.Dtos;

public class GetSurveyAnswersBySurveyIdDto
{
    public GetSurveyAnswersBySurveyIdDto()
    {
        SurveyAnswers = new List<GetSurveyAnswerBySurveyIdDto>();
    }

    public List<GetSurveyAnswerBySurveyIdDto> SurveyAnswers { get; set; }
}