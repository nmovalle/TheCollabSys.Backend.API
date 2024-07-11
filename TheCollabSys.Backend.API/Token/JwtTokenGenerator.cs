using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Response;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Token;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _config;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IUserService userService, ITokenService tokenService, IConfiguration config)
    {
        _userService = userService;
        _tokenService = tokenService;
        _config = config;
        _jwtSettings = config.GetSection("Jwt").Get<JwtSettings>();

        ValidateJwtSettings(_jwtSettings);
    }

    public async Task<AuthTokenResponse> GenerateToken(string username)
    {
        var user = await _userService.GetUserByName(username) ?? throw new UnauthorizedAccessException("User was not found.");

        var (token, tokenExpires) = GenerateJwtToken(user.Id.ToString());
        var refreshToken = GenerateRefreshToken();

        await SaveRefreshToken(user.Id, refreshToken);

        return CreateAuthTokenResponse(token, tokenExpires, refreshToken);
    }

    public async Task<AuthTokenResponse> RefreshToken(string refreshToken)
    {
        var token = await _tokenService.GetTokenAsync(refreshToken) ?? throw new UnauthorizedAccessException("Invalid or expired refresh token.");

        var user = await _userService.GetUserByIdAsync(token.UserId) ?? throw new UnauthorizedAccessException("User not found.");

        var (newAccessToken, tokenExpires) = GenerateJwtToken(user.Id.ToString());
        var newRefreshToken = GenerateRefreshToken();

        await SaveRefreshToken(user.Id, newRefreshToken);

        return CreateAuthTokenResponse(newAccessToken, tokenExpires, newRefreshToken);
    }

    private (string Token, DateTime Expires) GenerateJwtToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId) }),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return (tokenHandler.WriteToken(token), tokenDescriptor.Expires.Value);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private async Task SaveRefreshToken(string userId, string refreshToken)
    {
        var tokenDto = new AspNetUserTokensDTO
        {
            UserId = userId,
            LoginProvider = "TheCollabsysProvider",
            Name = "RefreshToken",
            Value = refreshToken
        };

        await _tokenService.InsertOrUpdateTokenAsync(userId, tokenDto);
    }

    private AuthTokenResponse CreateAuthTokenResponse(string accessToken, DateTime accessTokenExpiration, string refreshToken)
    {
        return new AuthTokenResponse
        {
            AccessToken = accessToken,
            AccessTokenExpiration = accessTokenExpiration,
            RefreshToken = refreshToken
        };
    }

    private void ValidateJwtSettings(JwtSettings jwtSettings)
    {
        if (string.IsNullOrEmpty(jwtSettings.SecretKey) ||
            string.IsNullOrEmpty(jwtSettings.Issuer) ||
            string.IsNullOrEmpty(jwtSettings.Audience) ||
            jwtSettings.ExpireMinutes <= 0)
        {
            throw new InvalidOperationException("Invalid JWT configuration.");
        }
    }
}

public class JwtSettings
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpireMinutes { get; set; }
}
