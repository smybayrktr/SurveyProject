using AutoMapper;
using SurveyProject.Mvc.Dtos;
using SurveyProject.Mvc.Models;

namespace SurveyProject.Mvc.Extensions;

public static class MappingExtensions
{
    public static LoginDto ConvertToDto(this LoginViewModel loginViewModel, IMapper mapper) =>
        mapper.Map<LoginDto>(loginViewModel);
    
    public static RegisterDto ConvertToDto(this RegisterViewModel registerViewModel, IMapper mapper) =>
        mapper.Map<RegisterDto>(registerViewModel);
    
    public static SurveyViewModel ConvertToViewModel(this GetSurveyByUrlDto getSurveyByUrlDto, IMapper mapper) =>
        mapper.Map<SurveyViewModel>(getSurveyByUrlDto);

    public static MySurveysViewModel ConvertToViewModel(this GetSurveysByUserIdDto getSurveysByUserIdDto, IMapper mapper) =>
        mapper.Map<MySurveysViewModel>(getSurveysByUserIdDto);
    
    public static GetSurveyAnswersBySurveyIdViewModel ConvertToViewModel(this GetSurveyAnswersBySurveyIdDto getSurveyAnswersBySurveyIdDto, IMapper mapper) =>
        mapper.Map<GetSurveyAnswersBySurveyIdViewModel>(getSurveyAnswersBySurveyIdDto);
    
    public static CreateSurveyAnswersDto ConvertToDto(this CreateSurveyAnswersViewModel createSurveyAnswersViewModel, IMapper mapper) =>
        mapper.Map<CreateSurveyAnswersDto>(createSurveyAnswersViewModel);
    
    public static CreateFilledSurveyDto ConvertToDto(this SurveyViewModel surveyViewModel, IMapper mapper) =>
        mapper.Map<CreateFilledSurveyDto>(surveyViewModel);
}