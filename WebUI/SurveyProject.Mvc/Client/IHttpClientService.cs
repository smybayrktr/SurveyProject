namespace SurveyProject.Mvc.Client;

public interface IHttpClientService : IDisposable
{
    Task<T> GetAsync<T>(string url, bool isAuthenticationRequired = false);
    Task<T> PostAsync<T>(string url, HttpContent content, bool isAuthenticationRequired = false);

}