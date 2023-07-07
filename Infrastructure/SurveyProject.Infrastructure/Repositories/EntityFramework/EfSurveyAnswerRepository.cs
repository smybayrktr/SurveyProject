using SurveyProject.Entities;
using SurveyProject.Infrastructure.Data;

namespace SurveyProject.Infrastructure.Repositories.EntityFramework;

public class EfSurveyAnswerRepository : EfEntityRepositoryBase<SurveyAnswer, SurveyContext>, ISurveyAnswerRepository
{
    public EfSurveyAnswerRepository(SurveyContext surveyContext) : base(surveyContext)
    {
    }
}