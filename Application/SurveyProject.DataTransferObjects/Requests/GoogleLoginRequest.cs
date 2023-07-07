namespace SurveyProject.DataTransferObjects.Requests;

public class GoogleLoginRequest
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
}