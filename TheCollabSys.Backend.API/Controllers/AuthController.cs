using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TheCollabSys.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("google")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GoogleResponse)),
                // Puedes agregar aquí propiedades adicionales según tus necesidades
            };

            return Challenge(properties, "Google");
        }

        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync();

            // Aquí puedes acceder a la información del usuario autenticado
            var userEmail = authenticateResult.Principal.FindFirst(ClaimTypes.Email)?.Value;
            // Realiza tus validaciones aquí

            return Ok(new { Email = userEmail });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            // Eliminar todas las cookies
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            // Devolver una respuesta indicando que las cookies se han eliminado
            return Ok("Cookies de autenticación eliminadas correctamente");
        }
    }
}
