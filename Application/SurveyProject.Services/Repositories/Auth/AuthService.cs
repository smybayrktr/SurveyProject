using System.Security.Claims;
using SurveyProject.DataTransferObjects.Requests;
using SurveyProject.Entities;
using SurveyProject.Services.Extensions;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using SurveyProject.Services.Repositories.AppUser;
using SurveyProject.Core.Utilities.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using SurveyProject.Core.Constants;
using SurveyProject.Core.Helpers.StringHelper;
using SurveyProject.Core.Utilities.Results;
using SurveyProject.Services.Repositories.Schedule;

namespace SurveyProject.Services.Repositories.Auth;

public class AuthService : IAuthService
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly TokenOption _tokenOption;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public AuthService(IPasswordHasher<User> passwordHasher,
        IUserService userService, IConfiguration configuration, IMapper mapper)
    {
        _passwordHasher = passwordHasher;
        _userService = userService;
        _tokenOption = configuration.GetSection("TokenOption").Get<TokenOption>();
        _mapper = mapper;
    }

    public async Task<IDataResult<AccessToken>> Register(UserRegisterRequest userRegisterRequest)
    {
        var checkUserByEmail = await checkUserExistsByEmail(userRegisterRequest.Email);
        if (checkUserByEmail.Success)
            return new ErrorDataResult<AccessToken>(ApiMessages.EmailAlreadyRegistered, ApiStatusCodes.BadRequest);

        var user = userRegisterRequest.ConvertToDto(_mapper);
        await _userService.CreateAsync(user, userRegisterRequest.Password);


        var userClaims = GetClaims(user);
        var token = CreateAccessToken(userClaims);
        var accessToken = new AccessToken
        {
            Token = token.Token,
            Expiration = token.Expiration
        };

        return new SuccessDataResult<AccessToken>(accessToken, ApiStatusCodes.Ok);
    }

    public async Task<IDataResult<AccessToken>> Login(UserLoginRequest userLoginRequest)
    {
        var userToFind = await _userService.GetByEmailAsync(userLoginRequest.Email);
        if (userToFind.Data == null || !userToFind.Success)
            return new ErrorDataResult<AccessToken>(ApiMessages.UserNotFound, ApiStatusCodes.BadRequest);

        var checkPassword =
            verifyUserPassword(userToFind.Data, userToFind.Data.PasswordHash, userLoginRequest.Password);
        if (!checkPassword.Success)
            return new ErrorDataResult<AccessToken>(ApiMessages.WrongEmailOrPassword, ApiStatusCodes.BadRequest);

        var userClaims = GetClaims(userToFind.Data);
        var token = CreateAccessToken(userClaims);
        var accessToken = new AccessToken
        {
            Token = token.Token,
            Expiration = token.Expiration
        };

        return new SuccessDataResult<AccessToken>(accessToken, ApiStatusCodes.Ok);
    }

    public async Task<IDataResult<AccessToken>> GoogleLogin(GoogleLoginRequest googleLoginRequest)
    {
        var checkEmailRegistered = await checkUserExistsByEmail(googleLoginRequest.Email);
        if (!checkEmailRegistered.Success)
        {
            var user = new User
            {
                Email = googleLoginRequest.Email,
                Name = googleLoginRequest.Name,
                LastName = googleLoginRequest.LastName
            };

            var userPassword = StringHelper.GenerateRandomPassword();
            await _userService.CreateAsync(user, userPassword);

            var checkUserAddLogin = await _userService.GetLoginsAsync(user);
            if (!checkUserAddLogin.Any())
            {
                await _userService.AddLoginAsync(user,new UserLoginInfo("Google", googleLoginRequest.Id, "Google"));
            }
            
            ScheduleService.ScheduleSendRegisterEmailWithPassword(user.Name, user.LastName,
                user.Email, userPassword);
            
            var userClaims = GetClaims(user);
            var accessToken = CreateAccessToken(userClaims);

            return new SuccessDataResult<AccessToken>(accessToken, ApiStatusCodes.Ok);
        }
        else
        {
            var checkUser = await _userService.GetByEmailAsync(googleLoginRequest.Email);
            var checkUserAddLogin = await _userService.GetLoginsAsync(checkUser.Data);
            if (!checkUserAddLogin.Any())
            {
                await _userService.AddLoginAsync(checkUser.Data, new UserLoginInfo("Google", googleLoginRequest.Id, "Google"));
            }
            
            var userClaims = GetClaims(checkUser.Data);
            var accessToken = CreateAccessToken(userClaims);

            return new SuccessDataResult<AccessToken>(accessToken, ApiStatusCodes.Ok);
        }
    }

    public async Task<IResult> ValidateToken(string token)
    {
        if (String.IsNullOrEmpty(token))
            return new ErrorResult(ApiStatusCodes.BadRequest);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenOption.SecurityKey);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            return new SuccessResult(ApiStatusCodes.Ok);
        }
        catch (Exception e)
        {
            return new ErrorResult(ApiStatusCodes.Unauthorized);
        }
    }

    private IEnumerable<Claim> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Role, "User"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        return claims;
    }

    private AccessToken CreateAccessToken(IEnumerable<Claim> claims)
    {
        var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpirationInMinutes);

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOption.SecurityKey));
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _tokenOption.Issuer,
            expires: accessTokenExpiration,
            notBefore: DateTime.Now,
            claims: claims,
            signingCredentials: signingCredentials,
            audience: _tokenOption.Audience
        );
        var handler = new JwtSecurityTokenHandler();
        var token = handler.WriteToken(jwtSecurityToken);
        return new AccessToken
        {
            Token = token,
            Expiration = accessTokenExpiration
        };
    }

    private async Task<IResult> checkUserExistsByEmail(string email)
    {
        var checkUserByEmail = await _userService.GetByEmailAsync(email);
        return checkUserByEmail.Data == null
            ? new ErrorResult(ApiStatusCodes.NotFound)
            : new SuccessResult(ApiStatusCodes.Ok);
    }

    private IResult verifyUserPassword(User user, string hashedPassword, string providedPassword)
    {
        var verifyPassword = _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        return verifyPassword == PasswordVerificationResult.Failed
            ? new ErrorResult(ApiStatusCodes.BadRequest)
            : new SuccessResult(ApiStatusCodes.Ok);
    }
}