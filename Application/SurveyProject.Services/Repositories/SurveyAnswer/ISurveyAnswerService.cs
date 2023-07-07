using SurveyProject.Core.Utilities.Results;
using SurveyProject.DataTransferObjects.Requests;
using SurveyProject.DataTransferObjects.Responses;

namespace SurveyProject.Services.Repositories.SurveyAnswer;

public interface ISurveyAnswerService
{
	Task<IResult> CreateAnswersAsync(CreateSurveyAnswersRequest createSurveyAnswersRequest);
	Task<IDataResult<GetSurveyAnswersBySurveyIdResponse>> GetSurveyAnswersBySurveyIdAsync(int surveyId);

}