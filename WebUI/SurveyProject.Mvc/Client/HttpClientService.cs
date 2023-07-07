using Newtonsoft.Json;
using SurveyProject.Mvc.Helpers.CookieHelper;

namespace SurveyProject.Mvc.Client;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;
    private readonly ICookieHelper _cookieHelper;

    public HttpClientService(HttpClient httpClient, ICookieHelper cookieHelper)
    {
        _httpClient = httpClient;
        _cookieHelper = cookieHelper;
    }

    public async Task<T> GetAsync<T>(string url, bool isAuthenticationRequired = false)
    {
        PrepareJwtIfRequired(isAuthenticationRequired);
        var response = await _httpClient.GetAsync(url);
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseContent);
    }

    public async Task<T> PostAsync<T>(string url, HttpContent content, bool isAuthenticationRequired = false)
    {
        PrepareJwtIfRequired(isAuthenticationRequired);
        var response = await _httpClient.PostAsync(url, content);
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseContent);
    }

    private void PrepareJwtIfRequired(bool isAuthenticationRequired)
    {
        if (isAuthenticationRequired)
        {
            var accessToken = _cookieHelper.GetJwtFromCookie();
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
        }
    }

    public void Dispose()
    {
        _httpClient.Dispose();
    }
}