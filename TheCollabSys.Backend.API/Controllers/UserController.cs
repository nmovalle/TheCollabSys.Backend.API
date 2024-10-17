using Azure.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.API.Extensions;
using TheCollabSys.Backend.API.Filters;
using TheCollabSys.Backend.API.Token;
using TheCollabSys.Backend.Entity.Auth;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using TheCollabSys.Backend.Entity.Response;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(GlobalExceptionFilter))]
[ServiceFilter(typeof(ModelStateFilter))]
public class UserController : BaseController
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    private readonly IUserRoleService _userRoleService;
    private readonly IUserCompanyService _userCompanyService;
    private readonly IWireListService _wireListService;
    private readonly IMapperService<UserDTO, AspNetUser> _mapperService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public UserController(
        ILogger<UserController> logger, 
        IUserService userService,
        IUserRoleService userRoleService,
        IUserCompanyService userCompanyService,
        IWireListService wireListService,
        IMapperService<UserDTO, AspNetUser> mapperService,
        IJwtTokenGenerator jwtTokenGenerator
        )
    {
        _logger = logger;
        _userService = userService;
        _userRoleService = userRoleService;
        _userCompanyService = userCompanyService;
        _wireListService = wireListService;
        _mapperService = mapperService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return await ExecuteWithCompanyIdAsync(async (companyId) =>
        {
            var data = await _userService.GetAll(companyId).ToListAsync();

            if (data.Any())
                return CreateResponse("success", data, "success");

            return CreateNotFoundResponse<object>(null, "Registers not founds");
        });
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        return await ExecuteWithCompanyIdAsync(async (companyId) =>
        {
            var data = await _userService.GetByIdAsync(companyId, id);

            if (data == null)
                return CreateNotFoundResponse<object>(null, "register not found");

            return CreateResponse("success", data, "success");
        });
    }

    [HttpGet("GetUserByName/{username}", Name = "GetUserByName")]
    [ActionName(nameof(GetUserByName))]
    public async Task<IActionResult> GetUserByName(string username)
    {
        var entity = await _userService.GetUserByName(username);
        return Ok(entity);
    }

    [HttpPost]
    public async Task<IActionResult> AddUserAsync([FromForm] string dto)
    {
        return await this.HandleClientOperationAsync<UserDTO>(dto, null, async (model) =>
        {
            if (model.UserName == null) return BadRequest();
            var existing = await _userService.GetUserByName(model.UserName);
            if (existing != null) return BadRequest("The user is already exist");

            model.Id = Guid.NewGuid().ToString();

            var entity = _mapperService.MapToDestination(model);
            var savedEntity = await _userService.AddUserAsync(entity);
            return CreateResponse("success", savedEntity, "success");
        });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserCreationModel model)
    {
        if (!IsValidRequest(model))
            return BadRequest();

        var userCreated = await this.AddNewUser(model.Email, model.Password);
        if (userCreated == null)
            return StatusCode(500, "Failed to add user");

        var newUserRole = await this.AddNewUserRole(userCreated.Id);
        if (newUserRole == null)
            return StatusCode(500, "Failed to add user role");

        var user = await this.GetUser(model.Email);

        return Ok(user);
    }

    [HttpPost("simple-register")]
    public async Task<IActionResult> RegisterWithoutPass([FromBody] UserWithoutPassModel model)
    {
        if (!IsValidRequest(model))
            return BadRequest();

        var user = await GetUser(model.Email);
        if (user == null)
        {
            var userCreated = await this.AddNewUser(model.Email);
            if (userCreated == null)
                return StatusCode(500, "Failed to add user");

            var newUserRole = await this.AddNewUserRole(userCreated.Id);
            if (newUserRole == null)
                return StatusCode(500, "Failed to add user role");

            user = await this.GetUser(model.Email);
        }

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var isAuthenticated = await _userService.SignInAsync(model.UserName, model.Password);

        if (isAuthenticated)
        {
            var userRole = await GetUserRole(model.UserName);
            if (userRole == null) return NotFound("user role not found.");

            var token = await GenerateToken(model.UserName);

            var usercompany = await GetUserCompany(userRole.UserId);
            if (usercompany == null) return NotFound("user company not found.");

            return Ok(new LoginResponse { UserRole = userRole, AuthToken = token, UserCompany = usercompany });
        }
        else
        {
            return Unauthorized("Invalid username or password");
        }
    }

    [HttpPost("updatepassword")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePassword model)
    {
        if (model == null)
            return BadRequest();

        var user = await _userService.GetByIdAsync(model.Id);
        if (user == null)
            return NotFound("User was not found");

        var userToUpdate = new AspNetUser()
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.UserName
        };

        await _userService.UpdatePasswordAsync(userToUpdate, model.NewPassword);

        var wireList = await _wireListService.GetByEmail(user.Email);
        wireList.PasswordConfirmed = true;
        await _wireListService.Update(wireList.Id, wireList);

        return Ok("Password updated successfully");
    }

    //[HttpPut("UpdateUserAsync/{id}")]
    //[ActionName(nameof(UpdateUserAsync))]
    //public async Task<IActionResult> UpdateUserAsync(string id, UserDTO dto)
    //{
    //    var existing = await _userService.GetUserByIdAsync(id);
    //    if (existing == null) return NotFound();

    //    await _userService.UpdateUserAsync(id, dto);

    //    return NoContent();
    //}

    [HttpDelete("DeleteUserAsync/{id}")]
    [ActionName(nameof(DeleteUserAsync))]
    public async Task<IActionResult> DeleteUserAsync(string id)
    {
        var existing = await _userService.GetByIdAsync(id);
        if (existing == null) return NotFound();

        await _userService.DeleteUserAsync(id);

        return NoContent();
    }

    private bool IsValidRequest(UserCreationModel request)
    {
        return request.UserName != null && request.Password != null;
    }

    private bool IsValidRequest(UserWithoutPassModel request)
    {
        return request.UserName != null;
    }
    private async Task<AspNetUser> AddNewUser(string email, string password)
    {
        var newUser = new AspNetUser()
        {
            Id = Guid.NewGuid().ToString(),
            UserName = email,
            Email = email
        };

        var addUserResult = await _userService.AddUserPasswordAsync(newUser, password);
        return addUserResult;
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
    private async Task<UserDTO?> GetUser(string email)
    {
        return await _userService.GetUserByName(email);
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
