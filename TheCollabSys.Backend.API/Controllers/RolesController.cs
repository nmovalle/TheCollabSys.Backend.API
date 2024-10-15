using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.API.Extensions;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : BaseController
{
    private readonly ILogger<RolesController> _logger;
    private readonly IRoleService _service;
    private readonly IMapperService<RoleDTO, AspNetRole> _mapperService;

    public RolesController(ILogger<RolesController> logger, IRoleService roleService, IMapperService<RoleDTO, AspNetRole> mapperService)
    {
        _logger = logger;
        _service = roleService;
        _mapperService = mapperService;
    }

    [HttpGet("GetAllRolesAsync", Name = "GetAllRolesAsync")]
    [ActionName(nameof(GetAllRolesAsync))]
    public async Task<IActionResult> GetAllRolesAsync()
    {
        return await ExecuteAsync(async () =>
        {
            var data = await _service.GetAll().ToListAsync();

            if (data.Any())
                return CreateResponse("success", data, "success");

            return CreateNotFoundResponse<object>(null, "Registers not founds");
        });
    }

    [HttpGet("GetRoleByIdAsync/{id}", Name = "GetRoleByIdAsync")]
    [ActionName(nameof(GetRoleByIdAsync))]
    public async Task<IActionResult> GetRoleByIdAsync(string id)
    {
        return await ExecuteAsync(async () =>
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
                return CreateNotFoundResponse<object>(null, "register not found");

            return CreateResponse("success", data, "success");
        });
    }

    [HttpGet("GetRoleByNameAsync/{name}", Name = "GetRoleByNameAsync")]
    [ActionName(nameof(GetRoleByNameAsync))]
    public async Task<IActionResult> GetRoleByNameAsync(string name)
    {
        var entity = await _service.GetRoleByNameAsync(name);
        return Ok(entity);
    }
}
