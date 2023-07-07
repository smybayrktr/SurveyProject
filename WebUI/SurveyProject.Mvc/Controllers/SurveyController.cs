using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SurveyProject.Mvc.Constants;
using SurveyProject.Mvc.Client;
using SurveyProject.Mvc.Models;
using SurveyProject.Mvc.Dtos;
using SurveyProject.Mvc.Helpers.StringContentHelper;
using IUrlHelper = SurveyProject.Mvc.Helpers.UrlHelper.IUrlHelper;
using Newtonsoft.Json;
using SurveyProject.Mvc.Extensions;

namespace SurveyProject.Mvc.Controllers;

public class SurveyController : Controller
{
    private readonly IHttpClientService _httpClientService;
    private readonly IUrlHelper _urlHelper;
    private readonly IMapper _mapper;

    public SurveyController(IHttpClientService httpClientService, IUrlHelper urlHelper, IMapper mapper)
    {
        _httpClientService = httpClientService;
        _urlHelper = urlHelper;
        _mapper = mapper;
    }

    [HttpGet("survey/create-survey")]
    public IActionResult CreateSurvey()
    {
        return View();
    }

    [HttpPost("survey/create-survey")]
    public async Task<IActionResult> CreateSurvey(SurveyViewModel surveyViewModel)
    {
        var createFilledSurveyDto = surveyViewModel.ConvertToDto(_mapper);

        var result = await _httpClientService.PostAsync<ApiResponse<string>>(Endpoints.CreateSurvey,
            StringContentHelper.CreateStringContent(createFilledSurveyDto), true);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message ?? "");
            return View(surveyViewModel);
        }

        var surveyCreatedViewModel = new SurveyCreatedViewModel
        {
            Url = _urlHelper.CreateSurveyUrl("survey/get-survey/" + result.Data)
        };

        return Json(new
        {
            redirectToUrl = Url.Action("SurveyCreated", "Survey",
                new { surveyCreatedViewModel = JsonConvert.SerializeObject(surveyCreatedViewModel) })
        });
    }

    [HttpGet("survey/survey-created")]
    public IActionResult SurveyCreated(string surveyCreatedViewModel)
    {
        var model = JsonConvert.DeserializeObject<SurveyCreatedViewModel>(surveyCreatedViewModel);

        return View(model);
    }

    [HttpGet("survey/get-survey/{url}")]
    public async Task<IActionResult> GetSurvey(string url)
    {
        var result = await _httpClientService.GetAsync<ApiResponse<GetSurveyByUrlDto>>(Endpoints.GetSurveyByUrl + url);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            return RedirectToAction("Index", "Home");
        }

        var surveyViewModel = result.Data.ConvertToViewModel(_mapper);
        return View(surveyViewModel);
    }

    [HttpPost("survey/create-survey-answers")]
    public async Task<IActionResult> CreateSurveyAnswers(CreateSurveyAnswersViewModel createSurveyAnswersViewModel)
    {
        var createSurveyAnswersDto = createSurveyAnswersViewModel.ConvertToDto(_mapper);

        var result =
            await _httpClientService.PostAsync<ApiResponse>(Endpoints.CreateSurveyAnswers,
                StringContentHelper.CreateStringContent(createSurveyAnswersDto));

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message ?? "");
            return RedirectToAction("Index", "Home");
        }

        return Json(new
        {
            redirectToUrl = Url.Action("SurveyAnswersCreated", "Survey")
        });
    }

    [HttpGet("survey/survey-answers-created")]
    public IActionResult SurveyAnswersCreated()
    {
        return View();
    }

    [HttpGet("survey/my-surveys")]
    public async Task<IActionResult> MySurveys()
    {
        var result =
            await _httpClientService.GetAsync<ApiResponse<GetSurveysByUserIdDto>>(Endpoints.GetSurveysByUserId,
                true);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message ?? "");
            return RedirectToAction("Index", "Home");
        }

        var mySurveysViewModel = result.Data.ConvertToViewModel(_mapper);

        return View(mySurveysViewModel);
    }

    [HttpGet("survey/my-surveys/{surveyId}")]
    public async Task<IActionResult> MySurveyStatistics(int surveyId)
    {
        var result =
            await _httpClientService.GetAsync<ApiResponse<GetSurveyAnswersBySurveyIdDto>>(
                Endpoints.GetSurveysBySurveyId + surveyId, true);

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message ?? "");
            return RedirectToAction("Index", "Home");
        }

        var getSurveyAnswersByIdViewModel = result.Data.ConvertToViewModel(_mapper);
        return View(getSurveyAnswersByIdViewModel);
    }

    [HttpGet("survey/get-survey-question-option")]
    public IActionResult GetSurveyQuestionOption(int questionType, int count, bool renderFirstTime)
    {
        return ViewComponent("SurveyQuestionOption",
            new { questionType = questionType, count = count, renderFirstTime = renderFirstTime });
    }

    [HttpGet("survey/get-survey-question")]
    public IActionResult GetSurveyQuestion(int questionType, int count)
    {
        return ViewComponent("SurveyQuestion", new { questionType = questionType, count = count });
    }
}