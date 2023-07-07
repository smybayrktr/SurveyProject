using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyProject.Core.Utilities.Results;
using SurveyProject.DataTransferObjects.Requests;
using SurveyProject.DataTransferObjects.Responses;
using SurveyProject.Services.Repositories.Survey;
using SurveyProject.Services.Repositories.SurveyAnswer;
using IResult = SurveyProject.Core.Utilities.Results.IResult;

namespace SurveyProject.WebApi.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("/api/v1/survey")]
public class SurveyController : Controller
{
    private readonly ISurveyService _surveyService;
    private readonly ISurveyAnswerService _surveyAnswerService;

    public SurveyController(ISurveyService surveyService, ISurveyAnswerService surveyAnswerService)
    {
        _surveyService = surveyService;
        _surveyAnswerService = surveyAnswerService;
    }

    [HttpPost("create-survey")]
    public async Task<IDataResult<string>> CreateSurvey(CreateFilledSurveyRequest createFilledSurveyRequest)
    {
        return await _surveyService.CreateAsync(createFilledSurveyRequest);
    }

    [AllowAnonymous]
    [HttpGet("get-by-url/{url}")]
    public async Task<IDataResult<GetSurveyByUrlResponse>> GetSurveyByUrl(string url)
    {
        return await _surveyService.GetByUrlAsync(url);
    }

    [AllowAnonymous]
    [HttpPost("create-survey-answers")]
    public async Task<IResult> CreateSurveyAnswers(CreateSurveyAnswersRequest createSurveyAnswersRequest)
    {
        return await _surveyAnswerService.CreateAnswersAsync(createSurveyAnswersRequest);
    }
    
    [HttpGet("get-by-user-id")]
    public async Task<IDataResult<GetSurveysByUserIdResponse>> GetByUserId()
    {
        return await _surveyService.GetByUserIdAsync();
    }

    [HttpGet("get-survey-answers/{surveyId}")]
    public async Task<IDataResult<GetSurveyAnswersBySurveyIdResponse>> GetSurveyAnswers(int surveyId)
    {
        return await _surveyAnswerService.GetSurveyAnswersBySurveyIdAsync(surveyId);
    }
}