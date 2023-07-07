using System.ComponentModel.DataAnnotations;

namespace SurveyProject.Mvc.Models;

public class RegisterViewModel
{
	public string Name { get; set; }

	public string LastName { get; set; }

	[Required]
	public string Email { get; set; }

	[Required]
	[DataType(DataType.Password)]
	public string Password { get; set; }
}