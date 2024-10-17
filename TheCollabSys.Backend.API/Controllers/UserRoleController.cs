using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.API.Extensions;
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
    public class UserRoleController : BaseController
    {
        private readonly ILogger<UserRoleController> _logger;
        private readonly IUserRoleService _userRoleService;
        private readonly IUserCompanyService _userCompanyService;
        private readonly IMapperService<UserRoleDTO, AspNetUserRole> _mapperService;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public UserRoleController(ILogger<UserRoleController> logger, IUserRoleService userRoleService, IUserCompanyService userCompanyService, IMapperService<UserRoleDTO, AspNetUserRole> mapperService, IJwtTokenGenerator jwtTokenGenerator)
        {
            _logger = logger;
            _userRoleService = userRoleService;
            _userCompanyService = userCompanyService;
            _mapperService = mapperService;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return await ExecuteWithCompanyIdAsync(async (companyId) =>
            {
                var data = await _userRoleService.GetAll(companyId).ToListAsync();

                if (data.Any())
                    return CreateResponse("success", data, "success");

                return CreateNotFoundResponse<object>(null, "Registers not founds");
            });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            return await ExecuteAsync(async () =>
            {
                var data = await _userRoleService.GetByIdAsync(id);

                if (data == null)
                    return CreateNotFoundResponse<object>(null, "register not found");

                return CreateResponse("success", data, "success");
            });
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

            if (updatedEntity == null) return NotFound();

            var userRole = await GetUserRole(username);
            if (userRole == null) return NotFound("user role not found.");

            var token = await GenerateToken(username);

            var usercompany = await GetUserCompany(userRole.UserId);
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

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(string id, [FromForm] string dto)
        {
            return await HandleClientOperationAsync<UserRoleDTO>(dto, null, async (model) =>
            {
                await _userRoleService.UpdateUserRoleByUserName(model.Email, model.RoleId);
                return NoContent();
            });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteMenu(string id)
        {
            try
            {
                await _userRoleService.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return CreateNotFoundResponse<object>(null, "register not found");
            }
        }
    }
}
