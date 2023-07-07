using System.Text;
using Newtonsoft.Json;

namespace SurveyProject.Mvc.Helpers.StringContentHelper;

public static class StringContentHelper
{
	public static StringContent CreateStringContent(object data)
	{
		return new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
	}
}