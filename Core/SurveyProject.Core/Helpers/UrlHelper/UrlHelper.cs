using Microsoft.Extensions.Configuration;

namespace SurveyProject.Core.Helpers.UrlHelper;

public class UrlHelper : IUrlHelper
{
    private string _baseAppUrl = "";

    public UrlHelper(IConfiguration configuration)
    {
        _baseAppUrl = configuration.GetSection("AppUrl").Value;
    }

    public string CreateSurveyUrl(string url)
    {
        if (!String.IsNullOrEmpty(url))
        {
            if (!url.Contains(_baseAppUrl))
            {
                if (_baseAppUrl.Contains("host.docker.internal"))
                {
                    _baseAppUrl = _baseAppUrl.Replace("host.docker.internal", "localhost");
                }
                if (url.Contains("host.docker.internal"))
                {
                    url = url.Replace("host.docker.internal", "localhost");
                }
                url = _baseAppUrl + url;
            }
        }

        return url;
    }
}