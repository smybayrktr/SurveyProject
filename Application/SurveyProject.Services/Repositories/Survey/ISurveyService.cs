using SurveyProject.Core.Utilities.Results;
using SurveyProject.DataTransferObjects.Requests;
using SurveyProject.DataTransferObjects.Responses;

namespace SurveyProject.Services.Repositories.Survey;

public interface ISurveyService
{
	Task<IDataResult<string>> CreateAsync(CreateFilledSurveyRequest createFilledSurveyRequest);
	Task<IDataResult<GetSurveyByUrlResponse>> GetByUrlAsync(string url);
	Task<IDataResult<GetSurveysByUserIdResponse>> GetByUserIdAsync();
	Task<IResult> CheckSurveyIsBelongsToCurrentUser(int surveyId);
}