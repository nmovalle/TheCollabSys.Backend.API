using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace TheCollabSys.Backend.API.Configuration
{
    public class AutenticationServiceIstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            // Configuración de autenticación
            services.AddAuthentication(options =>
            {
                // Esquema de autenticación predeterminado para cookies
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // Esquema predeterminado para iniciar sesión
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // Esquema predeterminado para el desafío de autenticación
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
            .AddCookie() // Agrega autenticación basada en cookies
            .AddJwtBearer(options =>
            {
                // Configuración JWT Bearer
                options.TokenValidationParameters.ValidateIssuer = true;
                options.TokenValidationParameters.ValidateAudience = true;
                options.TokenValidationParameters.ValidateIssuerSigningKey = true;
                options.TokenValidationParameters.ValidateLifetime = true;
                options.TokenValidationParameters.RequireExpirationTime = true;
                options.TokenValidationParameters.ValidIssuer = configuration["Jwt:Issuer"];
                options.TokenValidationParameters.ValidAudience = configuration["Jwt:Audience"];
                options.TokenValidationParameters.IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:SecretKey"]));
            })
            .AddGoogle(googleOptions => // Agrega autenticación de Google
            {
                googleOptions.ClientId = configuration["Authentication:Google:ClientId"];
                googleOptions.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                googleOptions.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                googleOptions.Events = new OAuthEvents
                {
                    // Personaliza los eventos si es necesario
                };
            });
        }
    }
}
