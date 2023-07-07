using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurveyProject.Entities;

namespace SurveyProject.Infrastructure.Data;

public class SurveyContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>
{
    public SurveyContext(DbContextOptions<SurveyContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Survey> Surveys { get; set; }
    public DbSet<SurveyAnswer> SurveyAnswers { get; set; }
    public DbSet<SurveyQuestion> SurveyQuestions { get; set; }
    public DbSet<SurveyQuestionOption> SurveyQuestionOptions { get; set; }
}