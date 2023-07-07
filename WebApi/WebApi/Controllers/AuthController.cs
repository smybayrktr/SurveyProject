using Microsoft.AspNetCore.Mvc;
using SurveyProject.Core.Utilities.Jwt;
using SurveyProject.Core.Utilities.Results;
using SurveyProject.DataTransferObjects.Requests;
using SurveyProject.Services.Repositories.Auth;

namespace SurveyProject.WebApi.Controllers;

[ApiController]
[Route("/api/v1/auth")]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IDataResult<AccessToken>> Login(UserLoginRequest userLoginRequest)
    {
        return await _authService.Login(userLoginRequest);
    }

    [HttpPost("register")]
    public async Task<IDataResult<AccessToken>> Register(UserRegisterRequest userRegisterRequest)
    {
        return await _authService.Register(userRegisterRequest);
    }
    
    [HttpPost("google-login")]
    public async Task<IDataResult<AccessToken>> GoogleLogin(GoogleLoginRequest googleLoginRequest)
    {
        return await _authService.GoogleLogin(googleLoginRequest);
    }
}