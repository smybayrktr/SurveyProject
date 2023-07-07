using AutoMapper;
using SurveyProject.Core.Constants;
using SurveyProject.Core.Helpers.UrlHelper;
using SurveyProject.Core.Utilities.Results;
using SurveyProject.DataTransferObjects.Requests;
using SurveyProject.DataTransferObjects.Responses;
using SurveyProject.Entities.Enums;
using SurveyProject.Infrastructure.Repositories;
using SurveyProject.Services.Extensions;
using SurveyProject.Services.Repositories.AppUser;
using SurveyProject.Services.Repositories.Cache;
using SurveyProject.Services.Repositories.Schedule;
using SurveyProject.Services.Repositories.SurveyQuestion;
using SurveyProject.Services.Repositories.SurveyQuestionOption;

namespace SurveyProject.Services.Repositories.Survey;

public class SurveyService : ISurveyService
{
    private readonly ISurveyRespository _surveyRespository;
    private readonly ISurveyQuestionService _surveyQuestionService;
    private readonly ISurveyQuestionOptionService _surveyQuestionOptionService;
    private readonly IUserService _userService;
    private readonly ICacheService _cacheService;
    private readonly IUrlHelper _urlHelper;
    private readonly IMapper _mapper;

    public SurveyService(ISurveyRespository surveyRespository, ISurveyQuestionService surveyQuestionService,
        ISurveyQuestionOptionService surveyQuestionOptionService, IUserService userService, ICacheService cacheService, IUrlHelper urlHelper, IMapper mapper)
    {
        _surveyRespository = surveyRespository;
        _surveyQuestionService = surveyQuestionService;
        _surveyQuestionOptionService = surveyQuestionOptionService;
        _userService = userService;
        _cacheService = cacheService;
        _urlHelper = urlHelper;
        _mapper = mapper;
    }

    public async Task<IDataResult<string>> CreateAsync(CreateFilledSurveyRequest createFilledSurveyRequest)
    {
        var currentUser = await _userService.GetCurrentUser();
        if (currentUser.Data == null || !currentUser.Success)
            return new ErrorDataResult<string>(ApiMessages.Unauthorized, ApiStatusCodes.BadRequest);

        var createSurveyRequest = new CreateSurveyRequest
        {
            UserId = currentUser.Data.Id
        };

        var survey = createSurveyRequest.ConvertToDto(_mapper);
        survey.Url = GenerateSurveyUrl().ToUpper();
        await _surveyRespository.CreateAsync(survey);

        foreach (var surveyQuestion in createFilledSurveyRequest.SurveyQuestions)
        {
            var createSurveyQuestionRequest = new CreateSurveyQuestionRequest
            {
                Question = surveyQuestion.Question,
                QuestionType = surveyQuestion.QuestionType,
                SurveyId = survey.Id
            };

            var surveyQuestionId = await _surveyQuestionService.CreateAsync(createSurveyQuestionRequest);

            if (surveyQuestion.QuestionType == QuestionType.MultipleChoice)
            {
                foreach (var surveyQuestionOption in surveyQuestion.SurveyQuestionOptions)
                {
                    var createSurveyQuestionOptionRequest = new CreateSurveyQuestionOptionRequest
                    {
                        SurveyQuestionId = surveyQuestionId.Data,
                        Text = surveyQuestionOption.Text
                    };
                    await _surveyQuestionOptionService.CreateAsync(createSurveyQuestionOptionRequest);
                }
            }
        }

        ScheduleService.SendSurveyCreatedEmail(currentUser.Data.Name, currentUser.Data.LastName, _urlHelper.CreateSurveyUrl(survey.Url), currentUser.Data.Email);
        
        return new SuccessDataResult<string>(survey.Url, ApiMessages.SurveyCreated, ApiStatusCodes.Created);
    }

    public async Task<IDataResult<GetSurveyByUrlResponse>> GetByUrlAsync(string url)
    {
        var checkIfCached =
            await _cacheService.GetDataAsync<GetSurveyByUrlResponse>(string.Format(CacheKeys.GetSurveyByUrl, url));
        if (checkIfCached!=null)
        {
            return new SuccessDataResult<GetSurveyByUrlResponse>(checkIfCached,ApiStatusCodes.Ok);
        }
        
        if (string.IsNullOrEmpty(url))
        {
            return new ErrorDataResult<GetSurveyByUrlResponse>(ApiStatusCodes.BadRequest);
        }

        var getSurveyByUrlDto = await _surveyRespository.GetSurveyByUrl(url);
        var getSurveyByUrlResponse = getSurveyByUrlDto.ConvertToDto(_mapper);
        
        await _cacheService.SetDataAsync(string.Format(CacheKeys.GetSurveyByUrl, url), getSurveyByUrlResponse);
        
        return new SuccessDataResult<GetSurveyByUrlResponse>(getSurveyByUrlResponse, ApiStatusCodes.Ok);
    }

    public async Task<IDataResult<GetSurveysByUserIdResponse>> GetByUserIdAsync()
    {
        var currentUser = await _userService.GetCurrentUser();
        if (currentUser.Data == null || !currentUser.Success)
            return new ErrorDataResult<GetSurveysByUserIdResponse>(ApiMessages.Unauthorized, ApiStatusCodes.BadRequest);

        var surveys = await _surveyRespository.GetAllWithPredicateAsync(x => x.UserId == currentUser.Data.Id);

        var getSurveysByUserIdRespone = new GetSurveysByUserIdResponse();
        getSurveysByUserIdRespone.Surveys = surveys.Select(x => x.ConvertToDto(_mapper)).ToList();

        return new SuccessDataResult<GetSurveysByUserIdResponse>(getSurveysByUserIdRespone, ApiStatusCodes.Ok);
    }

    public async Task<IResult> CheckSurveyIsBelongsToCurrentUser(int surveyId)
    {
        var currentUser = await _userService.GetCurrentUser();
        if (currentUser.Data == null || !currentUser.Success)
            return new ErrorResult(ApiMessages.Unauthorized, ApiStatusCodes.BadRequest);

        var survey = await _surveyRespository.GetWithPredicateAsync(x => x.Id == surveyId);
        if (survey == null)
        {
            return new ErrorResult(ApiMessages.SurvetNotFound, ApiStatusCodes.NotFound);
        }

        var checkIfSurveyBelongsToCurrentUser = survey.UserId == currentUser.Data.Id;
        if (!checkIfSurveyBelongsToCurrentUser)
        {
            return new ErrorResult(ApiMessages.Unauthorized, ApiStatusCodes.BadRequest);
        }

        return new SuccessResult(ApiStatusCodes.Ok);
    }

    private string GenerateSurveyUrl()
    {
        return Guid.NewGuid().ToString("N").Substring(0, 8);
    }
}