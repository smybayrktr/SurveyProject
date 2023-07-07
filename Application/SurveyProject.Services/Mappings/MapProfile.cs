using SurveyProject.DataTransferObjects.Requests;
using SurveyProject.Entities;
using AutoMapper;
using SurveyProject.Core.JoinDtos;
using SurveyProject.DataTransferObjects.Responses;

namespace SurveyProject.Services.Mappings;

public class MapProfile : Profile
{
    public MapProfile()
    {
        //Neyi-neye dönüştüreceğimizi yazdık.
        CreateMap<UserRegisterRequest, User>();
        CreateMap<UserLoginRequest, User>();
        CreateMap<CreateSurveyQuestionOptionRequest, SurveyQuestionOption>();
        CreateMap<CreateSurveyQuestionRequest, SurveyQuestion>();
        CreateMap<CreateSurveyAnswerRequest, SurveyAnswer>();
        CreateMap<CreateSurveyRequest, Survey>();
        CreateMap<SurveyQuestion, GetSurveyQuestionByIdResponse>();
        CreateMap<SurveyQuestionOption, GetSurveyQuestionOptionByIdResponse>();
        CreateMap<Survey, GetSurveyByUserIdResponse>();
        CreateMap<GetSurveyQuestionOptionByUrlDto, GetSurveyQuestionOptionByUrlResponse>();
        CreateMap<GetSurveyQuestionByUrlDto, GetSurveyQuestionByUrlResponse>();
        CreateMap<GetSurveyByUrlDto, GetSurveyByUrlResponse>();
    }
}