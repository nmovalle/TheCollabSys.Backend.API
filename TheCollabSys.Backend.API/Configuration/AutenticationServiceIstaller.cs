using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TheCollabSys.Backend.API.Token;

namespace TheCollabSys.Backend.API.Configuration;

public class AuthenticationServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSettings>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)),
                ClockSkew = TimeSpan.Zero // Elimina el tiempo de gracia por defecto (5 minutos)
            };
        })
        .AddCookie()
        .AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
            googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
            googleOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            googleOptions.Events = new OAuthEvents
            {
            };
        });
    }
}
