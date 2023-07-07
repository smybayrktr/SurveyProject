namespace SurveyProject.DataTransferObjects.Requests;

public class CreateFilledSurveyRequest
{
    public List<CreateFilledSurveyQuestionRequest> SurveyQuestions { get; set; }

    public CreateFilledSurveyRequest()
    {
        SurveyQuestions = new List<CreateFilledSurveyQuestionRequest>();
    }
}