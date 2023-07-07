using AutoMapper;
using SurveyProject.Core.Constants;
using SurveyProject.Core.Utilities.Results;
using SurveyProject.DataTransferObjects.Requests;
using SurveyProject.DataTransferObjects.Responses;
using SurveyProject.Infrastructure.Repositories;
using SurveyProject.Services.Extensions;

namespace SurveyProject.Services.Repositories.SurveyQuestion;

public class SurveyQuestionService: ISurveyQuestionService
{
    private readonly ISurveyQuestionRepository _surveyQuestionRepository;
    private readonly IMapper _mapper;

    public SurveyQuestionService(ISurveyQuestionRepository surveyQuestionRepository, IMapper mapper)
    {
        _surveyQuestionRepository = surveyQuestionRepository;
        _mapper = mapper;
    }

    public async Task<IDataResult<int>> CreateAsync(CreateSurveyQuestionRequest createSurveyQuestionRequest)
    {
        var surveyQuestion = createSurveyQuestionRequest.ConvertToDto(_mapper);
        await _surveyQuestionRepository.CreateAsync(surveyQuestion);
        return new SuccessDataResult<int>(surveyQuestion.Id, ApiStatusCodes.Created);
    }

    public async Task<IDataResult<IList<GetSurveyQuestionByIdResponse>>> GetBySurveyId(int id)
    {
        var result = await _surveyQuestionRepository.GetAllWithPredicateAsync(x => x.SurveyId == id);
        return new SuccessDataResult<IList<GetSurveyQuestionByIdResponse>>(result.Select(x=>x.ConvertToDto(_mapper)).ToList(), ApiStatusCodes.Ok);
    }

    public async Task<IDataResult<GetSurveyQuestionByIdResponse>> GetById(int id)
    {
        var result = await _surveyQuestionRepository.GetWithPredicateAsync(x => x.Id == id);
        return new SuccessDataResult<GetSurveyQuestionByIdResponse>(result.ConvertToDto(_mapper), ApiStatusCodes.Ok);
    }
}