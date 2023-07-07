using SurveyProject.Core.Utilities.Jwt;
using SurveyProject.Core.Utilities.Results;
using SurveyProject.DataTransferObjects.Requests;

namespace SurveyProject.Services.Repositories.Auth;

public interface IAuthService
{
    Task<IDataResult<AccessToken>> Register(UserRegisterRequest userRegisterRequest);
    Task<IDataResult<AccessToken>> Login(UserLoginRequest userLoginRequest);
    Task<IDataResult<AccessToken>> GoogleLogin(GoogleLoginRequest googleLoginRequest);
    Task<IResult> ValidateToken(string token);
}