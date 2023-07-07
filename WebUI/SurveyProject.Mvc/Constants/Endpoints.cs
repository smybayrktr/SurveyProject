using SurveyProject.Mvc.Utilities.IoC;

namespace SurveyProject.Mvc.Constants;

public static class Endpoints
{
	private static string BaseUrl = ServiceTool.ServiceProvider.GetService<IConfiguration>().GetSection("ApiUrl").Value;
	private static string BaseRoute = "/api/v1/";

	public static string Login = BaseUrl + BaseRoute + "auth/login";
	public static string Register = BaseUrl + BaseRoute + "auth/register";
	public static string GoogleLogin = BaseUrl + BaseRoute + "auth/google-login";


	public static string CreateSurvey = BaseUrl + BaseRoute + "survey/create-survey";
	public static string GetSurveyByUrl = BaseUrl + BaseRoute + "survey/get-by-url/";
	public static string CreateSurveyAnswers = BaseUrl + BaseRoute + "survey/create-survey-answers";
	public static string GetSurveysByUserId = BaseUrl + BaseRoute + "survey/get-by-user-id";
	public static string GetSurveysBySurveyId = BaseUrl + BaseRoute + "survey/get-survey-answers/";

}