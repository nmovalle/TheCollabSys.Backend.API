using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Token
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;

        public JwtTokenGenerator(IUserService userService, IConfiguration config)
        {
            _userService = userService;
            _config = config;
        }

        public async Task<string> GenerateToken(string username)
        {
            // Verificar si el usuario es válido
            var user = await _userService.GetUserByName(username);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User was not found.");
            }

            // Generar el token JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:SecretKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(
                    ClaimTypes.NameIdentifier, user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddHours(1), // El token expirará en 1 hora
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"]
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
