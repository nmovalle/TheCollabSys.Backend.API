using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.API.Extensions;
using TheCollabSys.Backend.API.Filters;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(GlobalExceptionFilter))]
[ServiceFilter(typeof(ModelStateFilter))]
[ServiceFilter(typeof(UserIdFilter))]
public class EmployerProjectAssignmentController : BaseController
{
    private readonly IEmployerProjectAssignmentService _service;
    public EmployerProjectAssignmentController(IEmployerProjectAssignmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEmployerProjectAssignments()
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
    [Route("{id}")]
    public async Task<IActionResult> GetEmployerProjectAssignmentsById(int id)
    {
        return await ExecuteAsync(async () =>
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
                return CreateResponse<object>("success", null, "register not found");

            return CreateResponse("success", data, "success");
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateEmployerProjectAssignment([FromForm] string dto, [FromForm] IFormFile? file)
    {
        return await this.HandleClientOperationAsync<EmployerProjectAssignmentDetailDTO>(dto, file, async (model) =>
        {
            await _service.Create(model);
            return NoContent();
        });
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateEmployerProjectAssignment(int id, [FromForm] string dto, [FromForm] IFormFile? file)
    {
        return await this.HandleClientOperationAsync<EmployerProjectAssignmentDetailDTO>(dto, file, async (model) =>
        {
            await _service.Update(id, model);
            return NoContent();
        });
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteEmployerProjectAssignment(int id)
    {
        try
        {
            await _service.Delete(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return CreateNotFoundResponse<object>(null, "register not found");
        }
    }
}
