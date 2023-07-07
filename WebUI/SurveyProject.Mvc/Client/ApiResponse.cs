namespace SurveyProject.Mvc.Client;

public class ApiResponse<T>
{
	public string? Message { get; set; }
	public T? Data { get; set; }
	public bool Success { get; set; }
	public int HttpStatusCode { get; set; }	
}

public class ApiResponse
{
	public string? Message { get; set; }
	public bool Success { get; set; }
	public int HttpStatusCode { get; set; }
}