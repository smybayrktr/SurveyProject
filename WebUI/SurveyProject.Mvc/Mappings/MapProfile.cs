using AutoMapper;
using SurveyProject.Mvc.Dtos;
using SurveyProject.Mvc.Models;

namespace SurveyProject.Mvc.Mappings;

public class MapProfile : Profile
{
    public MapProfile()
    {
        //Neyi-neye dönüştüreceğimizi yazdık.
        CreateMap<LoginViewModel, LoginDto>();
        CreateMap<RegisterViewModel, RegisterDto>();
        CreateMap<GetSurveyQuestionOptionByUrlDto, SurveyQuestionOptionViewModel>();
        CreateMap<GetSurveyQuestionByUrlDto, SurveyQuestionViewModel>();
        CreateMap<GetSurveyByUrlDto, SurveyViewModel>()
            .ForMember(dest => dest.SurveyQuestions,opt=>opt.MapFrom(src=>src.SurveyQuestions));
        CreateMap<GetSurveyByUserIdDto, MySurveyViewModel>();
        CreateMap<GetSurveysByUserIdDto, MySurveysViewModel>();
        CreateMap<GetSelectedSurveyAnswerDto, GetSelectedSurveyAnswerViewModel>();
        CreateMap<GetSurveyAnswerBySurveyIdDto, GetSurveyAnswerBySurveyIdViewModel>();
        CreateMap<GetSurveyAnswersBySurveyIdDto, GetSurveyAnswersBySurveyIdViewModel>();
        CreateMap<CreateSurveyAnswerViewModel, CreateSurveyAnswerDto>();
        CreateMap<CreateSurveyAnswersViewModel, CreateSurveyAnswersDto>();
        CreateMap<SurveyQuestionOptionViewModel, CreateFilledSurveyQuestionOptionDto>();
        CreateMap<SurveyQuestionViewModel, CreateFilledSurveyQuestionDto>();
        CreateMap<SurveyViewModel, CreateFilledSurveyDto>();
    }
}