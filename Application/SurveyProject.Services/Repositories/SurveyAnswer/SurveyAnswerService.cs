using AutoMapper;
using SurveyProject.Core.Constants;
using SurveyProject.Core.Utilities.Results;
using SurveyProject.DataTransferObjects.Requests;
using SurveyProject.DataTransferObjects.Responses;
using SurveyProject.Entities.Enums;
using SurveyProject.Infrastructure.Repositories;
using SurveyProject.Services.Extensions;
using SurveyProject.Services.Repositories.Survey;
using SurveyProject.Services.Repositories.SurveyQuestion;
using SurveyProject.Services.Repositories.SurveyQuestionOption;

namespace SurveyProject.Services.Repositories.SurveyAnswer;

public class SurveyAnswerService : ISurveyAnswerService
{
    private readonly ISurveyAnswerRepository _surveyAnswerRepository;
    private readonly ISurveyQuestionService _surveyQuestionService;
    private readonly ISurveyQuestionOptionService _surveyQuestionOptionService;
    private readonly ISurveyService _surveyService;
    private readonly IMapper _mapper;

    public SurveyAnswerService(ISurveyAnswerRepository surveyAnswerRepository,
        ISurveyQuestionOptionService surveyQuestionOptionService, ISurveyQuestionService surveyQuestionService,
        ISurveyService surveyService, IMapper mapper)
    {
        _surveyAnswerRepository = surveyAnswerRepository;
        _surveyQuestionOptionService = surveyQuestionOptionService;
        _surveyQuestionService = surveyQuestionService;
        _surveyService = surveyService;
        _mapper = mapper;
    }

    public async Task<IResult> CreateAnswersAsync(CreateSurveyAnswersRequest createSurveyAnswersRequest)
    {
        var surveyAnswers = createSurveyAnswersRequest.CreateSurveyAnswerRequests.Select(x => x.ConvertToDto(_mapper)).ToList();
        await _surveyAnswerRepository.CreateRangeAsync(surveyAnswers);
        
        return new SuccessResult(ApiStatusCodes.Created);
    }

    public async Task<IDataResult<GetSurveyAnswersBySurveyIdResponse>> GetSurveyAnswersBySurveyIdAsync(int surveyId)
    {
        var checkIfSurveyBelongsToCurrentUser = await _surveyService.CheckSurveyIsBelongsToCurrentUser(surveyId);
        if (!checkIfSurveyBelongsToCurrentUser.Success)
        {
            return new ErrorDataResult<GetSurveyAnswersBySurveyIdResponse>(ApiMessages.Unauthorized,
                ApiStatusCodes.Unauthorized);
        }

        var surveyQuestions = await _surveyQuestionService.GetBySurveyId(surveyId);
        var getSurveyAnswersByIdResponse = new GetSurveyAnswersBySurveyIdResponse();
        
        foreach (var surveyQuestion in surveyQuestions.Data)
        {
            var getSelectedSurveyAnswerResponses = new List<GetSelectedSurveyAnswerResponse>();
            var totalAnswer =
                (await _surveyAnswerRepository.GetAllWithPredicateAsync(x =>
                    x.SurveyQuestionId == surveyQuestion.Id)).Count;

            if (surveyQuestion.QuestionType == QuestionType.MultipleChoice)
            {
                var surveyQuestionOptions = await _surveyQuestionOptionService.GetBySurveyQuestionId(surveyQuestion.Id);

                foreach (var option in surveyQuestionOptions.Data)
                {
                    var surveyAnswersCount =
                        (await _surveyAnswerRepository.GetAllWithPredicateAsync(x =>
                            x.SurveyQuestionId == surveyQuestion.Id && x.MultipleChoiceAnswer == option.Id)).Count;

                    getSelectedSurveyAnswerResponses.Add(new GetSelectedSurveyAnswerResponse
                    {
                        Text = option.Text,
                        Count = surveyAnswersCount
                    });
                }

                var getSurveyAnswerByIdResponse = new GetSurveyAnswerBySurveyIdResponse
                {
                    Question = surveyQuestion.Question,
                    QuestionType = surveyQuestion.QuestionType,
                    SelectedSurveyAnswers = getSelectedSurveyAnswerResponses,
                    TotalAnswer = totalAnswer
                };

                getSurveyAnswersByIdResponse.SurveyAnswers.Add(getSurveyAnswerByIdResponse);
            }
            else if (surveyQuestion.QuestionType == QuestionType.SingleLinePlainText)
            {
                var surveyAnswers =
                    await _surveyAnswerRepository.GetAllWithPredicateAsync(x =>
                        x.SurveyQuestionId == surveyQuestion.Id);

                foreach (var item in surveyAnswers)
                {
                    getSelectedSurveyAnswerResponses.Add(new GetSelectedSurveyAnswerResponse
                    {
                        Text = item.SingleLinePlainTextAnswer
                    });
                }
                
                var getSurveyAnswerByIdResponse = new GetSurveyAnswerBySurveyIdResponse
                {
                    Question = surveyQuestion.Question,
                    QuestionType = surveyQuestion.QuestionType,
                    SelectedSurveyAnswers = getSelectedSurveyAnswerResponses,
                    TotalAnswer = totalAnswer
                };

                getSurveyAnswersByIdResponse.SurveyAnswers.Add(getSurveyAnswerByIdResponse);
            }
            else if (surveyQuestion.QuestionType == QuestionType.MultipleLinePlainText)
            {
                var surveyAnswers =
                    await _surveyAnswerRepository.GetAllWithPredicateAsync(x =>
                        x.SurveyQuestionId == surveyQuestion.Id);

                foreach (var item in surveyAnswers)
                {
                    getSelectedSurveyAnswerResponses.Add(new GetSelectedSurveyAnswerResponse
                    {
                        Text = item.MultipleLinePlainTextAnswer
                    });
                }
                
                var getSurveyAnswerByIdResponse = new GetSurveyAnswerBySurveyIdResponse
                {
                    Question = surveyQuestion.Question,
                    QuestionType = surveyQuestion.QuestionType,
                    SelectedSurveyAnswers = getSelectedSurveyAnswerResponses,
                    TotalAnswer = totalAnswer
                };

                getSurveyAnswersByIdResponse.SurveyAnswers.Add(getSurveyAnswerByIdResponse);
            }
            else if(surveyQuestion.QuestionType == QuestionType.Scoring)
            {
                var surveyAnswers =
                    await _surveyAnswerRepository.GetAllWithPredicateAsync(x =>
                        x.SurveyQuestionId == surveyQuestion.Id);

                foreach (var item in surveyAnswers)
                {
                    getSelectedSurveyAnswerResponses.Add(new GetSelectedSurveyAnswerResponse
                    {
                        Text = item.ScoringAnswer.ToString()
                    });
                }
                
                var getSurveyAnswerByIdResponse = new GetSurveyAnswerBySurveyIdResponse
                {
                    Question = surveyQuestion.Question,
                    QuestionType = surveyQuestion.QuestionType,
                    SelectedSurveyAnswers = getSelectedSurveyAnswerResponses,
                    TotalAnswer = totalAnswer
                };

                getSurveyAnswersByIdResponse.SurveyAnswers.Add(getSurveyAnswerByIdResponse);
            }
        }

        return new SuccessDataResult<GetSurveyAnswersBySurveyIdResponse>(getSurveyAnswersByIdResponse,
            ApiStatusCodes.Ok);
    }
}