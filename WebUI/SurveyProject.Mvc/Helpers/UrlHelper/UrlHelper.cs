namespace SurveyProject.Mvc.Helpers.UrlHelper;

public class UrlHelper : IUrlHelper
{
    private string _baseUrl = "";

    public UrlHelper(IConfiguration configuration)
    {
        _baseUrl = configuration.GetSection("BaseUrl").Value;
    }

    public string CreateSurveyUrl(string url)
    {
        if (!String.IsNullOrEmpty(url))
        {
            if (!url.Contains(_baseUrl))
            {
                if (_baseUrl.Contains("host.docker.internal"))
                {
                    _baseUrl = _baseUrl.Replace("host.docker.internal", "localhost");
                }
                if (url.Contains("host.docker.internal"))
                {
                    url = url.Replace("host.docker.internal", "localhost");
                }
                url = _baseUrl + url;
            }
        }

        return url;
    }
}