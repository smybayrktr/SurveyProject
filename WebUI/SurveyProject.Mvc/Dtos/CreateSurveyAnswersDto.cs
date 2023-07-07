namespace SurveyProject.Mvc.Dtos;

public class CreateSurveyAnswersDto
{
    public CreateSurveyAnswersDto()
    {
        CreateSurveyAnswerRequests = new List<CreateSurveyAnswerDto>();
    }

    public List<CreateSurveyAnswerDto> CreateSurveyAnswerRequests { get; set; }
}