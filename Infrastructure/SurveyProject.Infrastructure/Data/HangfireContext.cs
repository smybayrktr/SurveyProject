using Microsoft.EntityFrameworkCore;

namespace SurveyProject.Infrastructure.Data;

public class HangfireContext : DbContext
{
    public HangfireContext(DbContextOptions<HangfireContext> options) : base(options)
    {
    }
}