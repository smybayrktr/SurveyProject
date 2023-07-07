using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore;
using SurveyProject.Entities;
using SurveyProject.Core.Constants;
using IResult = SurveyProject.Core.Utilities.Results.IResult;
using Microsoft.Net.Http.Headers;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SurveyProject.Core.Utilities.Jwt;
using Microsoft.Extensions.Configuration;
using SurveyProject.Core.Utilities.Results;

namespace SurveyProject.Services.Repositories.AppUser;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TokenOption _tokenOption;

    public UserService(UserManager<User> userManager,
        IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _tokenOption = configuration.GetSection("TokenOption").Get<TokenOption>();
    }

    public async Task<IDataResult<IList<User?>>> GetAllAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        return new SuccessDataResult<IList<User?>>(users, ApiStatusCodes.Ok);
    }

    public async Task<IDataResult<User?>> GetByIdAsync(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        return new SuccessDataResult<User>(user,ApiStatusCodes.Ok);
    }

    public async Task<IDataResult<User?>> GetByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return new SuccessDataResult<User>(user,ApiStatusCodes.Ok);
    }

    public async Task<IResult> CreateAsync(User user, string password)
    {
        user.UserName = user.Email;
        user.SecurityStamp = Guid.NewGuid().ToString();
        var createResult = await _userManager.CreateAsync(user,password);
        if (!createResult.Succeeded)
            return new ErrorResult(ApiStatusCodes.InternalServerError);
            
        var addClaimResult = await AddUserClaimsAsync(user);
        if (!addClaimResult.Success)
            return addClaimResult;

        return new SuccessResult(ApiStatusCodes.Created);
    }

    public async Task<IResult> UpdateAsync(User user)
    {
        await _userManager.UpdateAsync(user);
        return new SuccessResult(ApiStatusCodes.Ok);
    }
    
    public async Task<IResult> DeleteAsync(User user)
    {
        await _userManager.DeleteAsync(user);
        return new SuccessResult(ApiStatusCodes.Ok);
    }

    private async Task<IResult> AddUserClaimsAsync(User user)
    {
        var claims = new List<Claim>();
        claims.Add(new Claim(ClaimTypes.Name, user.Email));
        claims.Add(new Claim(ClaimTypes.Role, "User"));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        await _userManager.AddClaimsAsync(user, claims);
        return new SuccessResult(ApiStatusCodes.Ok);
    }

    public async Task<IDataResult<User?>> GetCurrentUser()
    {
        var jwt = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization];
        if (String.IsNullOrEmpty(jwt)) return new ErrorDataResult<User>(ApiStatusCodes.NotFound);

        jwt = jwt.ToString().Replace("Bearer ","");

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenOption.SecurityKey);

        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false
        };

        var claimsPrincipal = tokenHandler.ValidateToken(jwt, tokenValidationParameters, out _);
            
        string userEmail = claimsPrincipal?.FindFirstValue(ClaimTypes.Email);
        if (String.IsNullOrWhiteSpace(userEmail)) return new ErrorDataResult<User>(ApiStatusCodes.NotFound);

        return await GetByEmailAsync(userEmail);

    }

    public async Task<IdentityResult> AddLoginAsync(User user, UserLoginInfo userLoginInfo)
    {
        return await _userManager.AddLoginAsync(user, userLoginInfo);
    }
    
    public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
    {
        return await _userManager.GetLoginsAsync(user);
    }

    public async Task<IdentityResult> AddClaimsAsync(User user, IEnumerable<Claim> claims)
    {
        return await _userManager.AddClaimsAsync(user, claims);
    }
}