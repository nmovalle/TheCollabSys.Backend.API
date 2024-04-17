using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.API.Filters;
using TheCollabSys.Backend.API.Token;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using TheCollabSys.Backend.Entity.Response;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(GlobalExceptionFilter))]
    [ServiceFilter(typeof(ModelStateFilter))]
    public class UserRoleController : ControllerBase
    {
        private readonly ILogger<UserRoleController> _logger;
        private readonly IUserRoleService _userRoleService;
        private readonly IMapperService<UserRoleDTO, AspNetUserRole> _mapperService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UserRoleController(ILogger<UserRoleController> logger, IUserRoleService userRoleService, IMapperService<UserRoleDTO, AspNetUserRole> mapperService, IJwtTokenGenerator jwtTokenGenerator)
        {
            _logger = logger;
            _userRoleService = userRoleService;
            _mapperService = mapperService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost]
        [ActionName(nameof(AddUserRoleAsync))]
        public async Task<IActionResult> AddUserRoleAsync(UserRoleDTO dto)
        {

            var entity = _mapperService.MapToDestination(dto);
            var savedEntity = await _userRoleService.AddUserRoleAsync(entity);

            return Ok(savedEntity);
        }

        [HttpGet("GetUserRoleByUserName/{username}", Name = "GetUserRoleByUserName")]
        [ActionName(nameof(GetUserRoleByUserName))]
        public async Task<IActionResult> GetUserRoleByUserName(string username)
        {
            var entity = await _userRoleService.GetUserRoleByUserName(username);
            return Ok(entity);
        }

        [HttpPut("UpdateUserRoleByUserName/{username}/{newRoleId}", Name = "UpdateUserRoleByUserName")]
        [ActionName(nameof(UpdateUserRoleByUserName))]
        public async Task<IActionResult> UpdateUserRoleByUserName(string username, string newRoleId)
        {
            var updatedEntity = await _userRoleService.UpdateUserRoleByUserName(username, newRoleId);

            if (updatedEntity == null)
            {
                return NotFound();
            }


            // Obtener el usuario con su rol
            var userRole = await GetUserRole(username);
            var token = await GenerateToken(username);

            return Ok(new LoginResponse { UserRole = userRole, AuthToken = token });
        }

        private async Task<UserRoleDTO?> GetUserRole(string username)
        {
            return await _userRoleService.GetUserRoleByUserName(username);
        }

        private async Task<AuthTokenResponse> GenerateToken(string username)
        {
            return await _jwtTokenGenerator.GenerateToken(username);
        }
    }
}
