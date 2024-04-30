using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly ILogger<RolesController> _logger;
    private readonly IRoleService _roleService;
    private readonly IMapperService<RoleDTO, AspNetRole> _mapperService;

    public RolesController(ILogger<RolesController> logger, IRoleService roleService, IMapperService<RoleDTO, AspNetRole> mapperService)
    {
        _logger = logger;
        _roleService = roleService;
        _mapperService = mapperService;
    }

    [HttpGet("GetAllRolesAsync", Name = "GetAllRolesAsync")]
    [ActionName(nameof(GetAllRolesAsync))]
    public async Task<IActionResult> GetAllRolesAsync()
    {
        return Ok(await _roleService.GetAllRolesAsync());
    }

    [HttpGet("GetRoleByIdAsync/{id}", Name = "GetRoleByIdAsync")]
    [ActionName(nameof(GetRoleByIdAsync))]
    public async Task<IActionResult> GetRoleByIdAsync(string id)
    {
        var entity = await _roleService.GetRoleByIdAsync(id);
        if (entity == null)
            return NotFound();

        return Ok(entity);
    }

    [HttpGet("GetRoleByNameAsync/{name}", Name = "GetRoleByNameAsync")]
    [ActionName(nameof(GetRoleByNameAsync))]
    public async Task<IActionResult> GetRoleByNameAsync(string name)
    {
        var entity = await _roleService.GetRoleByNameAsync(name);
        return Ok(entity);
    }
}
