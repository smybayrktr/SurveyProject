namespace SurveyProject.Mvc.Dtos;

public class GetSurveysByUserIdDto
{
    public GetSurveysByUserIdDto()
    {
        Surveys = new List<GetSurveyByUserIdDto>();
    }
    
    public List<GetSurveyByUserIdDto> Surveys { get; set; }
}