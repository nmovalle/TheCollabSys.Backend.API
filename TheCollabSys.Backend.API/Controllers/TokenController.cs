using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.API.Token;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Request;
using TheCollabSys.Backend.Entity.Response;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserService _userService;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserCompanyService _userCompanyService;

        public TokenController(IJwtTokenGenerator jwtTokenGenerator, IUserRoleService userRoleService, IUserService userService, IUserCompanyService userCompanyService)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRoleService = userRoleService;
            _userService = userService;
            _userCompanyService = userCompanyService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> GenerateTokenAsync(AuthenticationRequestBody authenticationRequestBody)
        {
            if (string.IsNullOrEmpty(authenticationRequestBody.UserName)) return BadRequest("username is required.");

            var token = await _jwtTokenGenerator.GenerateToken(authenticationRequestBody.UserName);

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("refreshtoken")]
        public async Task<IActionResult> RefreshToken(AuhenticationRefreshRequestBody auhenticationRefreshRequestBody)
        {
            //if (string.IsNullOrEmpty(auhenticationRefreshRequestBody.username)) return BadRequest("username is required.");
            if (string.IsNullOrEmpty(auhenticationRefreshRequestBody.refreshToken)) return BadRequest("refresh is required.");

            var token = await _jwtTokenGenerator.RefreshToken(auhenticationRefreshRequestBody.refreshToken);

            return Ok(token);
        }

        [AllowAnonymous]
        [HttpPost("token-domain")]
        public async Task<IActionResult> GenerateTokenDomain(AuthenticationDomainRequestBody body)
        {
            if (string.IsNullOrEmpty(body.UserId)) return BadRequest("userid is required.");

            var user = await _userService.GetByIdAsync(body.UserId);
            if (user == null) return NotFound("user not found.");

            var userRole = await GetUserRole(user.UserName);
            if (userRole == null) return NotFound("user role not found.");

            var token = await GenerateToken(user.UserName);

            var usercompany = await GetUserCompany(user.Id);
            if (usercompany == null) return NotFound("user company not found.");

            return Ok(new LoginResponse { UserRole = userRole, AuthToken = token, UserCompany = usercompany });
        }

        private async Task<UserRoleDTO?> GetUserRole(string username)
        {
            return await _userRoleService.GetUserRoleByUserName(username);
        }

        private async Task<AuthTokenResponse> GenerateToken(string username)
        {
            return await _jwtTokenGenerator.GenerateToken(username);
        }

        private async Task<UserCompanyDTO?> GetUserCompany(string userid)
        {
            return await _userCompanyService.GetByUserIdAsync(userid);
        }
    }
}
