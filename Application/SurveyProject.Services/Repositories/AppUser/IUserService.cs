using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using SurveyProject.Core.Utilities.Results;
using SurveyProject.Entities;

namespace SurveyProject.Services.Repositories.AppUser;

public interface IUserService
{
    Task<IDataResult<IList<User?>>> GetAllAsync();
    Task<IDataResult<User?>> GetByIdAsync(int id);
    Task<IResult> CreateAsync(User user, string password);
    Task<IResult> UpdateAsync(User user);
    Task<IResult> DeleteAsync(User user);
    Task<IDataResult<User?>> GetByEmailAsync(string email);
    Task<IDataResult<User?>> GetCurrentUser();
    Task<IdentityResult> AddLoginAsync(User user, UserLoginInfo userLoginInfo);
    Task<IList<UserLoginInfo>> GetLoginsAsync(User user);
    Task<IdentityResult> AddClaimsAsync(User user, IEnumerable<Claim> claims);
}