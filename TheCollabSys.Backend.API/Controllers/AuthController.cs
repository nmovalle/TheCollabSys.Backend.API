using Azure.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TheCollabSys.Backend.API.Extensions;
using TheCollabSys.Backend.API.Token;
using TheCollabSys.Backend.Entity.Auth;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using TheCollabSys.Backend.Entity.Request;
using TheCollabSys.Backend.Entity.Response;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;

        private readonly IDomainService _domainService;
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IMenuRolesService _menuRolesService;
        private readonly IEmailService _emailService;
        private readonly IAccessCodeService _accessCodeService;
        private readonly IUserCompanyService _userCompanyService;

        public AuthController(
            ILogger<AuthController> logger,
            IConfiguration configuration,
            IDomainService domainService, 
            IUserService userService, 
            IUserRoleService userRoleService,
            IJwtTokenGenerator jwtTokenGenerator,
            IMenuRolesService menuRolesService,
            IEmailService emailService,
            IAccessCodeService accessCodeService,
            IUserCompanyService userCompanyService
            )
        {
            _logger = logger;
            _configuration = configuration;
            _domainService = domainService;
            _userService = userService;
            _userRoleService = userRoleService;
            _jwtTokenGenerator = jwtTokenGenerator;
            _menuRolesService = menuRolesService;
            _emailService = emailService;
            _accessCodeService = accessCodeService;
            _userCompanyService = userCompanyService;
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

            //validar existencia de dominio
            var domainMaster = await GetDomainMaster(request.hd);
            if (domainMaster == null)
            {
                var success = await this.ResendAccessCode(request.email);
                if (success)
                {
                    return NotFound(new { Type = "domain-master", Email = request.email });
                }
                return StatusCode(500, "Failed to generate access code");
            }

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

            //validar si el usuario ya pertenece a un dominio/company
            var usercompany = await GetUserCompany(user.Id);
            if (usercompany == null)
            {
                return NotFound(new { Type = "user-company", Email = request.email, UserId = user.Id });
            }

            // Obtener el usuario con su rol
            var userRole = await GetUserRole(user.UserName);
            var token = await GenerateToken(user.UserName);

            return Ok(new LoginResponse { UserRole= userRole, AuthToken = token });
        }

        private bool IsValidRequest(OAuthRequest request)
        {
            return request.hd != null && request.email != null;
        }

        private async Task<DomainMasterDTO?> GetDomainMaster(string domain)
        {
            return await _domainService.GetByDomainAsync(domain);
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

        private async Task<UserCompanyDTO?> GetUserCompany(string userid)
        {
            return await _userCompanyService.GetByUserIdAsync(userid);
        }

        [HttpGet]
        [Route("menus")]
        public async Task<IActionResult> GetAllMenuRoles()
        {
            var data = await _menuRolesService.GetAll().ToListAsync();
            return Ok(data);
        }

        [HttpGet]
        [Route("menus/{username}")]
        public async Task<IActionResult> GetAllMenuRolesById(string username)
        {
            var user = await _userRoleService.GetUserRoleByUserName(username);
            if (user == null) { return NotFound(new { Email = username }); }

            var data = await _menuRolesService.GetByRole(user.RoleId).ToListAsync();
            return Ok(data);
        }

        [HttpPost("GetUserByName")]
        public async Task<IActionResult> GetUserByName(string username)
        {

            return Ok(username);
        }

        #endregion

        #region Token
        private async Task<AuthTokenResponse> GenerateToken(string username) 
        {
            return await _jwtTokenGenerator.GenerateToken(username);
        }
        private async Task<AuthTokenResponse> GenerateRefreshToken(string refreshToken)
        {
            return await _jwtTokenGenerator.RefreshToken(refreshToken);
        }
        #endregion

        #region Access Code
        private string GenerateAccessCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }
        private async Task UpdateExistingAccessCode(AccessCodeDTO existingAccessCode, string newAccessCode)
        {
            int expireMinutes = _configuration.GetValue<int>("AccessCode:ExpireMinutes");
            var currentTime = DateTime.UtcNow;
            existingAccessCode.AccessCode = newAccessCode;
            existingAccessCode.RegAt = currentTime;
            existingAccessCode.ExpAt = currentTime.AddMinutes(expireMinutes);
            existingAccessCode.IsValid = false;

            await _accessCodeService.Update(existingAccessCode.Id, existingAccessCode);
        }
        private async Task<DdAccessCode> CreateNewAccessCode(string email, string newAccessCode)
        {
            int expireMinutes = _configuration.GetValue<int>("AccessCode:ExpireMinutes");
            var currentTime = DateTime.UtcNow;
            var ddAccessCode = new DdAccessCode
            {
                Email = email,
                AccessCode = newAccessCode,
                RegAt = currentTime,
                ExpAt = currentTime.AddMinutes(expireMinutes),
                IsValid = false
            };

            return await _accessCodeService.Create(ddAccessCode);
        }
        private async Task SendAccessCodeEmail(string email, string accessCode)
        {
            var subject = "Your Access Code";
            var body = $"<p>Your access code is: <strong>{accessCode}</strong></p>";
            await _emailService.SendEmailAsync(email, subject, body);
        }
        private async Task<bool> ResendAccessCode(string email)
        {
            try
            {
                var existingAccessCode = await _accessCodeService.GetByEmail(email);
                var newAccessCode = GenerateAccessCode();

                if (existingAccessCode != null)
                {
                    await UpdateExistingAccessCode(existingAccessCode, newAccessCode);
                }
                else
                {
                    await CreateNewAccessCode(email, newAccessCode);
                }

                await SendAccessCodeEmail(email, newAccessCode);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al reenviar el código de acceso: {ex.Message}");
                return false;
            }
        }
        #endregion
    }
}
