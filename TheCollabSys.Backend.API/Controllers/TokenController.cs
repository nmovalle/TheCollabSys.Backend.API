using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.API.Token;

namespace TheCollabSys.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public TokenController(IJwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpGet]
        public async Task<IActionResult> GenerateTokenAsync(string username)
        {
            // Verificar si el username es válido
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Nombre de usuario no válido.");
            }

            // Generar un nuevo token
            var token = await _jwtTokenGenerator.GenerateToken(username);

            // Devolver el token generado
            return Ok(token);
        }
    }
}
