using Azure.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    private readonly IUserRoleService _userRoleService;
    private readonly IMapperService<UserDTO, AspNetUser> _mapperService;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public UserController(
        ILogger<UserController> logger, 
        IUserService userService,
        IUserRoleService userRoleService,
        IMapperService<UserDTO, AspNetUser> mapperService,
        IJwtTokenGenerator jwtTokenGenerator
        )
    {
        _logger = logger;
        _userService = userService;
        _userRoleService = userRoleService;
        _mapperService = mapperService;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [HttpGet("GetAllUsersAsync", Name = "GetAllUsersAsync")]
    [ActionName(nameof(GetAllUsersAsync))]
    public async Task<IActionResult> GetAllUsersAsync()
    {
        return Ok(await _userService.GetAllUsersAsync());
    }

    [HttpGet("GetUserByIdAsync/{id}", Name = "GetUserByIdAsync")]
    [ActionName(nameof(GetUserByIdAsync))]
    public async Task<IActionResult> GetUserByIdAsync(string id)
    {
        var entity = await _userService.GetUserByIdAsync(id);
        if (entity == null)
            return NotFound();

        return Ok(entity);
    }

    [HttpGet("GetUserByName/{username}", Name = "GetUserByName")]
    [ActionName(nameof(GetUserByName))]
    public async Task<IActionResult> GetUserByName(string username)
    {
        var entity = await _userService.GetUserByName(username);
        return Ok(entity);
    }

    [HttpPost]
    [ActionName(nameof(AddUserAsync))]
    public async Task<IActionResult> AddUserAsync(UserDTO dto)
    {
        if (dto.UserName == null) return BadRequest();

        var existing = await _userService.GetUserByName(dto.UserName);
        if (existing != null) return BadRequest("The user is already exist");

        dto.Id = Guid.NewGuid().ToString();
        var entity = _mapperService.MapToDestination(dto);
        var savedEntity = await _userService.AddUserAsync(entity);
        var savedDTO = _mapperService.MapToSource(savedEntity);

        return CreatedAtAction(nameof(GetUserByIdAsync), new { id = savedDTO.Id }, savedDTO);
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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var isAuthenticated = await _userService.SignInAsync(model.UserName, model.Password);

        if (isAuthenticated)
        {
            // El usuario ha iniciado sesión exitosamente
            var userRole = await GetUserRole(model.UserName);
            var token = await GenerateToken(model.UserName);

            return Ok(new LoginResponse { UserRole = userRole, AuthToken = token });
        }
        else
        {
            // Las credenciales son inválidas
            return Unauthorized("Invalid username or password");
        }
    }

    [HttpPost("updatepassword")]
    public async Task<IActionResult> UpdatePassword([FromBody] UpdatePassword model)
    {
        if (model == null)
            return BadRequest();

        var user = await _userService.GetUserByIdAsync(model.Id);
        if (user == null)
            return NotFound("User was not found");

        var userToUpdate = new AspNetUser()
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.UserName
        };

        // Actualizar el password
        await _userService.UpdatePasswordAsync(userToUpdate, model.NewPassword);

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
        var existing = await _userService.GetUserByIdAsync(id);
        if (existing == null) return NotFound();

        await _userService.DeleteUserAsync(id);

        return NoContent();
    }

    private bool IsValidRequest(UserCreationModel request)
    {
        return request.UserName != null && request.Password != null;
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
}
