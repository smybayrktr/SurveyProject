using SurveyProject.Entities;
using SurveyProject.Infrastructure.Data;

namespace SurveyProject.Infrastructure.Repositories.EntityFramework;

public class EfSurveyQuestionRepository : EfEntityRepositoryBase<SurveyQuestion, SurveyContext>,
    ISurveyQuestionRepository
{
    public EfSurveyQuestionRepository(SurveyContext surveyContext) : base(surveyContext)
    {
    }
}