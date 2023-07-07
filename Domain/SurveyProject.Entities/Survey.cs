using SurveyProject.Core.Helpers.DateHelper;

namespace SurveyProject.Entities;

public class Survey: IEntity
{
	public int Id { get; set; }
	public int UserId { get; set; }
	public long CreatedAt { get; set; } = DateHelper.DateToTimestampt(DateTime.Now);
	public string Url { get; set; }
}