using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.API.Extensions;
using TheCollabSys.Backend.API.Filters;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(GlobalExceptionFilter))]
[ServiceFilter(typeof(ModelStateFilter))]
public class MenuRolesController : BaseController
{
    private readonly IMenuRolesService _service;
    public MenuRolesController(
        IMenuRolesService service
        )
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllMenuRoles()
    {
        return await ExecuteAsync(async () =>
        {
            var data = await _service.GetAll().ToListAsync();

            if (data.Any())
                return CreateResponse("success", data, "success");

            return CreateNotFoundResponse<object>(null, "Registers not founds");
        });
    }

    [HttpGet]
    [Route("{roleId}")]
    public async Task<IActionResult> GetAllMenuRolesById(string roleId)
    {
        return await ExecuteAsync(async () =>
        {
            var data = await _service.GetByRole(roleId).ToListAsync();

            if (data.Any())
                return CreateResponse("success", data, "success");

            return CreateNotFoundResponse<object>(null, "Registers not founds");
        });
    }
}
