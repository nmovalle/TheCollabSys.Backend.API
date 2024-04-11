using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.API.Filters;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
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

        public UserRoleController(ILogger<UserRoleController> logger, IUserRoleService userRoleService, IMapperService<UserRoleDTO, AspNetUserRole> mapperService)
        {
            _logger = logger;
            _userRoleService = userRoleService;
            _mapperService = mapperService;
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

        [HttpPost("UpdateUserRoleByUserName/{username}/{newRoleId}", Name = "UpdateUserRoleByUserName")]
        [ActionName(nameof(UpdateUserRoleByUserName))]
        public async Task<IActionResult> UpdateUserRoleByUserName(string username, string newRoleId)
        {
            var updatedEntity = await _userRoleService.UpdateUserRoleByUserName(username, newRoleId);

            if (updatedEntity == null)
            {
                return NotFound(); // Devolver un resultado NotFound si no se encuentra el usuario o no tiene roles asignados
            }

            return Ok(updatedEntity);
        }
    }
}
