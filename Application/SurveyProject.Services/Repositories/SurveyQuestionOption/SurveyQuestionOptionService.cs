using AutoMapper;
using SurveyProject.Core.Constants;
using SurveyProject.Core.Utilities.Results;
using SurveyProject.DataTransferObjects.Requests;
using SurveyProject.DataTransferObjects.Responses;
using SurveyProject.Infrastructure.Repositories;
using SurveyProject.Services.Extensions;

namespace SurveyProject.Services.Repositories.SurveyQuestionOption;

public class SurveyQuestionOptionService: ISurveyQuestionOptionService
{
    private readonly ISurveyQuestionOptionRepository _surveyQuestionOptionRepository;
    private readonly IMapper _mapper;

    public SurveyQuestionOptionService(ISurveyQuestionOptionRepository surveyQuestionOptionRepository, IMapper mapper)
    {
        _surveyQuestionOptionRepository = surveyQuestionOptionRepository;
        _mapper = mapper;
    }

    public async Task<IResult> CreateAsync(CreateSurveyQuestionOptionRequest createSurveyQuestionOptionRequest)
    {
        var surveyQuestionOption = createSurveyQuestionOptionRequest.ConvertToDto(_mapper);
        await _surveyQuestionOptionRepository.CreateAsync(surveyQuestionOption);
        return new SuccessResult(ApiStatusCodes.Created);
    }

    public async Task<IDataResult<IList<GetSurveyQuestionOptionByIdResponse>>> GetBySurveyQuestionId(int id)
    {
        var result = await _surveyQuestionOptionRepository.GetAllWithPredicateAsync(x => x.SurveyQuestionId == id);
        return new SuccessDataResult<IList<GetSurveyQuestionOptionByIdResponse>>(result.Select(x=>x.ConvertToDto(_mapper)).ToList(), ApiStatusCodes.Ok);
    }
}