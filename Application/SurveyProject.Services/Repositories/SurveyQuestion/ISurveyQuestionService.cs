using SurveyProject.Core.Utilities.Results;
using SurveyProject.DataTransferObjects.Requests;
using SurveyProject.DataTransferObjects.Responses;

namespace SurveyProject.Services.Repositories.SurveyQuestion;

public interface ISurveyQuestionService
{
	Task<IDataResult<int>> CreateAsync(CreateSurveyQuestionRequest createSurveyQuestionRequest);
	Task<IDataResult<IList<GetSurveyQuestionByIdResponse>>> GetBySurveyId(int id);
	Task<IDataResult<GetSurveyQuestionByIdResponse>> GetById(int id);
}