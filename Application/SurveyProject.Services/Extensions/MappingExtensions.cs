using SurveyProject.DataTransferObjects.Requests;
using SurveyProject.Entities;
using AutoMapper;
using SurveyProject.Core.JoinDtos;
using SurveyProject.DataTransferObjects.Responses;

namespace SurveyProject.Services.Extensions;

public static class MappingExtensions
{
    public static User ConvertToDto(this UserRegisterRequest userRegisterRequest, IMapper mapper) =>
        mapper.Map<User>(userRegisterRequest);

    public static SurveyQuestionOption ConvertToDto(
        this CreateSurveyQuestionOptionRequest createSurveyQuestionOptionRequest, IMapper mapper) =>
        mapper.Map<SurveyQuestionOption>(createSurveyQuestionOptionRequest);

    public static SurveyQuestion ConvertToDto(this CreateSurveyQuestionRequest createSurveyQuestionRequest,
        IMapper mapper) =>
        mapper.Map<SurveyQuestion>(createSurveyQuestionRequest);

    public static SurveyAnswer ConvertToDto(this CreateSurveyAnswerRequest createSurveyAnswerRequest,
        IMapper mapper) =>
        mapper.Map<SurveyAnswer>(createSurveyAnswerRequest);

    public static Survey ConvertToDto(this CreateSurveyRequest createSurveyRequest, IMapper mapper) =>
        mapper.Map<Survey>(createSurveyRequest);

    public static GetSurveyQuestionByIdResponse ConvertToDto(this SurveyQuestion surveyQuestionResponses,
        IMapper mapper) =>
        mapper.Map<GetSurveyQuestionByIdResponse>(surveyQuestionResponses);

    public static GetSurveyQuestionOptionByIdResponse ConvertToDto(
        this SurveyQuestionOption surveyQuestionResponses, IMapper mapper) =>
        mapper.Map<GetSurveyQuestionOptionByIdResponse>(surveyQuestionResponses);
    
    public static GetSurveyByUserIdResponse ConvertToDto(
        this Survey survey, IMapper mapper) =>
        mapper.Map<GetSurveyByUserIdResponse>(survey);
    
    public static GetSurveyByUrlResponse ConvertToDto(
        this GetSurveyByUrlDto getSurveyByUrlDto, IMapper mapper) =>
        mapper.Map<GetSurveyByUrlResponse>(getSurveyByUrlDto);
}