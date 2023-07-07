namespace SurveyProject.DataTransferObjects.Responses;

public class GetSurveysByUserIdResponse
{
    public GetSurveysByUserIdResponse()
    {
        Surveys = new List<GetSurveyByUserIdResponse>();
    }

    public List<GetSurveyByUserIdResponse> Surveys { get; set; }
}