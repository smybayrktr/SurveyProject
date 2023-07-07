using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using SurveyProject.Mvc.Constants;
using SurveyProject.Mvc.Client;
using SurveyProject.Mvc.Models;
using SurveyProject.Mvc.Dtos;
using SurveyProject.Mvc.Extensions;
using SurveyProject.Mvc.Helpers.CookieHelper;
using SurveyProject.Mvc.Helpers.StringContentHelper;

namespace SurveyProject.Mvc.Controllers;

public class AuthController : Controller
{
    private readonly IHttpClientService _httpClientService;
    private readonly ICookieHelper _cookieHelper;
    private readonly IMapper _mapper;

    public AuthController(IHttpClientService httpClientService, ICookieHelper cookieHelper, IMapper mapper)
    {
        _httpClientService = httpClientService;
        _cookieHelper = cookieHelper;
        _mapper = mapper;
    }

    [HttpGet("login")]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
            return View(loginViewModel);

        var loginDto = loginViewModel.ConvertToDto(_mapper);
        var result = await _httpClientService.PostAsync<ApiResponse<AccessTokenDto>>(Endpoints.Login,
            StringContentHelper.CreateStringContent(loginDto));

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            return View(loginViewModel);
        }

        _cookieHelper.SetJwtCookie(result.Data.Token);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("register")]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
            return View(registerViewModel);

        var registerDto = registerViewModel.ConvertToDto(_mapper);
        var result = await _httpClientService.PostAsync<ApiResponse<AccessTokenDto>>(Endpoints.Register,
            StringContentHelper.CreateStringContent(registerDto));

        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            return View(registerViewModel);
        }

        _cookieHelper.SetJwtCookie(result.Data.Token);
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        _cookieHelper.DeleteJwtCookie();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("google-login")]
    public IActionResult GoogleLogin()
    {
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = Url.Action(nameof(GoogleLoginCallback))
        };

        return Challenge(authenticationProperties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-login-callback")]
    public async Task<IActionResult> GoogleLoginCallback()
    {
        var authenticateResult = await HttpContext.AuthenticateAsync();
        if (!authenticateResult.Succeeded)
        {
            return RedirectToAction("Login", "Auth");
        }

        var id = authenticateResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = authenticateResult.Principal.FindFirstValue(ClaimTypes.Email);
        var firstName = authenticateResult.Principal.FindFirstValue(ClaimTypes.GivenName);
        var lastName = authenticateResult.Principal.FindFirstValue(ClaimTypes.Surname);

        var googleLoginDto = new GoogleLoginDto
        {
            Id = id,
            Email = email,
            Name = firstName,
            LastName = lastName
        };
        
        var result = await _httpClientService.PostAsync<ApiResponse<AccessTokenDto>>(Endpoints.GoogleLogin,
            StringContentHelper.CreateStringContent(googleLoginDto));
        
        if (!result.Success)
            return RedirectToAction("Login", "Auth");
        
        _cookieHelper.SetJwtCookie(result.Data.Token);
        return RedirectToAction("Index", "Home");
    }
}
