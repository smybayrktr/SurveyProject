namespace SurveyProject.DataTransferObjects.Responses;

public class GetSurveyAnswersBySurveyIdResponse
{
    public GetSurveyAnswersBySurveyIdResponse()
    {
        SurveyAnswers = new List<GetSurveyAnswerBySurveyIdResponse>();
    }

    public List<GetSurveyAnswerBySurveyIdResponse> SurveyAnswers { get; set; }
}