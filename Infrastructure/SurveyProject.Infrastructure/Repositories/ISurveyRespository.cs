using SurveyProject.Core.JoinDtos;
using SurveyProject.Entities;

namespace SurveyProject.Infrastructure.Repositories;

public interface ISurveyRespository : IEntityRepository<Survey>
{
    Task<GetSurveyByUrlDto> GetSurveyByUrl(string url);
}