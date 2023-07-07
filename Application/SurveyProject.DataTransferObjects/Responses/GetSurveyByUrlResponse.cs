namespace SurveyProject.DataTransferObjects.Responses;

public class GetSurveyByUrlResponse
{
    public GetSurveyByUrlResponse()
    {
        SurveyQuestions = new List<GetSurveyQuestionByUrlResponse>();
    }

    public List<GetSurveyQuestionByUrlResponse> SurveyQuestions { get; set; }
}