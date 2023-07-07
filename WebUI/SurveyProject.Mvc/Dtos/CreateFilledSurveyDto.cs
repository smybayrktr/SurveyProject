namespace SurveyProject.Mvc.Dtos;

public class CreateFilledSurveyDto
{
    public List<CreateFilledSurveyQuestionDto> SurveyQuestions { get; set; }

    public CreateFilledSurveyDto()
    {
        SurveyQuestions = new List<CreateFilledSurveyQuestionDto>();
    }
}