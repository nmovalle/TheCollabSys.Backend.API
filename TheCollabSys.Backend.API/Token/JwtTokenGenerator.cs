using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TheCollabSys.Backend.Entity.Response;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Token;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IUserService _userService;
    private readonly IConfiguration _config;
    private readonly JwtSettings _jwtSettings;

    public JwtTokenGenerator(IUserService userService, IConfiguration config)
    {
        _userService = userService;
        _config = config;
        _jwtSettings = config.GetSection("Jwt").Get<JwtSettings>();

        if (string.IsNullOrEmpty(_jwtSettings.SecretKey) ||
            string.IsNullOrEmpty(_jwtSettings.Issuer) ||
            string.IsNullOrEmpty(_jwtSettings.Audience) ||
            _jwtSettings.ExpireMinutes <= 0)
        {
            throw new InvalidOperationException("Invalid JWT configuration.");
        }
    }

    public async Task<AuthTokenResponse> GenerateToken(string username)
    {
        var user = await _userService.GetUserByName(username);
        if (user == null)
        {
            throw new UnauthorizedAccessException("User was not found.");
        }

        var tokenExpires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpireMinutes);
        var token = GenerateJwtToken(user.Id.ToString(), tokenExpires);

        return new AuthTokenResponse
        {
            AccessToken = token,
            AccessTokenExpiration = tokenExpires,
            RefreshToken = GenerateRefreshToken()
        };
    }

    private string GenerateJwtToken(string userId, DateTime expires)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            }),
            Expires = expires,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}

public class JwtSettings
{
    public string SecretKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpireMinutes { get; set; }
}
