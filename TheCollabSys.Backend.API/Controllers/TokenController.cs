using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.API.Token;
using TheCollabSys.Backend.Entity.Request;
using RefreshRequest = TheCollabSys.Backend.Entity.Request.RefreshRequest;

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

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> GenerateTokenAsync(AuthenticationRequestBody authenticationRequestBody)
        {
            // Verificar si el username es válido
            if (string.IsNullOrEmpty(authenticationRequestBody.UserName))
            {
                return BadRequest("Nombre de usuario no válido.");
            }

            // Generar un nuevo token
            var token = await _jwtTokenGenerator.GenerateToken(authenticationRequestBody.UserName);

            // Devolver el token generado
            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("refreshtoken")]
        public async Task<IActionResult> RefreshToken(RefreshRequest refreshRequest)
        {
            //pendiente registrar el user y token
            //obtener el token y usuario por el refreshToken
            //generar de nuevo el token: var token = await _jwtTokenGenerator.GenerateToken(authenticationRequestBody.UserName);
            return Ok();
        }
    }
}
