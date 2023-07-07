using SurveyProject.Core.JoinDtos;
using SurveyProject.Entities;
using SurveyProject.Infrastructure.Data;

namespace SurveyProject.Infrastructure.Repositories.EntityFramework;

public class EfSurveyRepository : EfEntityRepositoryBase<Survey, SurveyContext>, ISurveyRespository
{
    protected readonly SurveyContext _surveyContext;
    
    public EfSurveyRepository(SurveyContext surveyContext) : base(surveyContext)
    {
        _surveyContext = surveyContext;
    }
    
    public async Task<GetSurveyByUrlDto> GetSurveyByUrl(string url)
    {
        var surveyResult = _surveyContext.Surveys
            .Where(s => s.Url == url)
            .Select(s => new GetSurveyByUrlDto
            {
                SurveyQuestions = _surveyContext.SurveyQuestions
                    .Where(sq => sq.SurveyId == s.Id)
                    .Select(sq => new GetSurveyQuestionByUrlDto
                    {
                        Id = sq.Id,
                        Question = sq.Question,
                        QuestionType = (int)sq.QuestionType,
                        SurveyQuestionOptions = _surveyContext.SurveyQuestionOptions
                            .Where(sqo => sqo.SurveyQuestionId == sq.Id)
                            .Select(sqo => new GetSurveyQuestionOptionByUrlDto
                            {
                                Id = sqo.Id,
                                Text = sqo.Text
                            })
                            .ToList()
                    })
                    .ToList()
            })
            .FirstOrDefault();

        return surveyResult;
    }
}