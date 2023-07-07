using SurveyProject.Entities;
using SurveyProject.Infrastructure.Data;

namespace SurveyProject.Infrastructure.Repositories.EntityFramework;

public class EfSurveyQuestionOptionRepository : EfEntityRepositoryBase<SurveyQuestionOption, SurveyContext>,
    ISurveyQuestionOptionRepository
{
    public EfSurveyQuestionOptionRepository(SurveyContext surveyContext) : base(surveyContext)
    {
    }
}