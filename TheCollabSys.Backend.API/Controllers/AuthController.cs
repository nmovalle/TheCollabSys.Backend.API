using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TheCollabSys.Backend.Entity.Auth;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IDomainService _domainService;
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        public AuthController(ILogger<AuthController> logger, IDomainService domainService, IUserService userService, IUserRoleService userRoleService)
        {
            _logger = logger;
            _domainService = domainService;
            _userService = userService;
            _userRoleService = userRoleService;
        }

        #region Google
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
        #endregion

        #region Google OAuth2.0
        [HttpPost("validate-oauth-domain")]
        public async Task<IActionResult> ValidateOAuth(OAuthRequest request)
        {
            if (!IsValidRequest(request))
                return BadRequest();

            var domainMaster = await GetDomainMaster(request.hd);
            if (domainMaster == null)
                return NotFound();

            var user = await GetUser(request.email);
            if (user == null)
            {
                var newUserAdded = await AddNewUser(request.email);
                if (newUserAdded == null)
                    return StatusCode(500, "Failed to add user");

                var newUserRole = await AddNewUserRole(newUserAdded.Id);
                if (newUserRole == null)
                    return StatusCode(500, "Failed to add user role");

                user = await GetUser(request.email);
            }

            // Obtener el usuario con su rol
            var userRole = await GetUserRole(user.UserName);

            return Ok(userRole);
        }

        private bool IsValidRequest(OAuthRequest request)
        {
            return request.hd != null && request.email != null;
        }

        private async Task<DdDomainMaster?> GetDomainMaster(string domain)
        {
            return await _domainService.GetDomainMasterByDomain(domain);
        }

        #endregion

        #region User
        private async Task<UserDTO?> GetUser(string email)
        {
            return await _userService.GetUserByName(email);
        }

        private async Task<AspNetUser> AddNewUser(string email)
        {
            var newUser = new AspNetUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = email,
                Email = email
            };

            var addUserResult = await _userService.AddUserAsync(newUser);
            return addUserResult;
        }

        private async Task<AspNetUserRole> AddNewUserRole(string userid)
        {
            var newUserRole = new AspNetUserRole()
            {
                UserId = userid,
                RoleId = "8"
            };

            var saved = await _userRoleService.AddUserRoleAsync(newUserRole);
            return saved;
        }

        private async Task<UserRoleDTO?> GetUserRole(string username)
        {
            return await _userRoleService.GetUserRoleByUserName(username);
        }

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

        #region Token


        #endregion
    }
}
