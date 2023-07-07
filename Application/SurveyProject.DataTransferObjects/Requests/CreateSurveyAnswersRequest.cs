namespace SurveyProject.DataTransferObjects.Requests;

public class CreateSurveyAnswersRequest
{
    public CreateSurveyAnswersRequest()
    {
        CreateSurveyAnswerRequests = new List<CreateSurveyAnswerRequest>();
    }

    public List<CreateSurveyAnswerRequest> CreateSurveyAnswerRequests { get; set; }
}