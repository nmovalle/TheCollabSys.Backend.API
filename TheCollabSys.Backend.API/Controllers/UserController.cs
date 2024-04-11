using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.API.Filters;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
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
    private readonly IMapperService<UserDTO, AspNetUser> _mapperService;

    public UserController(ILogger<UserController> logger, IUserService userService, IMapperService<UserDTO, AspNetUser> mapperService)
    {
        _logger = logger;
        _userService = userService;
        _mapperService = mapperService;

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
}
