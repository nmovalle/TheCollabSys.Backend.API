using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TheCollabSys.Backend.Entity.Auth;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IDomainService _domainService;
        public AuthController(ILogger<AuthController> logger, IDomainService domainService)
        {
            _logger = logger;
            _domainService = domainService;
        }

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

        #region Google OAuth2.0
        [HttpPost("validate-oauth-domain")]
        public async Task<IActionResult> ValidateOAuth(OAuthRequest request)
        {
            //validar si existe el domain
            if (request.hd == null) return BadRequest();

            var domainMaster = await _domainService.GetDomainMasterByDomain(request.hd);
            if (domainMaster == null) return NotFound();

            //validar si existe el user


            //si no existe se crea


            //obtener el user con sus roles: [Id, UserName, Email, RoleId, RoleName, NormalizedRoleName]


            return Ok(request);
        }
        #endregion

        #region User
        [HttpPost("menus")]
        public async Task<IActionResult> GetMenus(string username)
        {

            return Ok(username);
        }

        [HttpPost("GetUserByName")]
        public async Task<IActionResult> GetUserByName(string username)
        {

            return Ok(username);
        }

        #endregion
    }
}
