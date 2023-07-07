using SurveyProject.Core.Utilities.Results;
using SurveyProject.DataTransferObjects.Requests;
using SurveyProject.DataTransferObjects.Responses;

namespace SurveyProject.Services.Repositories.SurveyQuestionOption;

public interface ISurveyQuestionOptionService
{
	Task<IResult> CreateAsync(CreateSurveyQuestionOptionRequest createSurveyQuestionOptionRequest);
	Task<IDataResult<IList<GetSurveyQuestionOptionByIdResponse>>> GetBySurveyQuestionId(int id);
}