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
[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(GlobalExceptionFilter))]
[ServiceFilter(typeof(ModelStateFilter))]
public class ProjectSkillsController : BaseController
{
    private readonly IProjectSkillService _service;
    private readonly IMapperService<ProjectSkillDTO, DdProjectSkill> _mapper;
    public ProjectSkillsController(
        IProjectSkillService service,
        IMapperService<ProjectSkillDTO, DdProjectSkill> mapperService
        )
    {
        _service = service;
        _mapper = mapperService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjectSkills()
    {
        return await ExecuteWithCompanyIdAsync(async (companyId) =>
        {
            var data = await _service.GetAll(companyId).ToListAsync();

            if (data.Any())
                return CreateResponse("success", data, "success");

            return CreateNotFoundResponse<object>(null, "Registers not founds");
        });
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetAllProjectSkillsById(int id)
    {
        return await ExecuteWithCompanyIdAsync(async (companyId) =>
        {
            var data = await _service.GetByIdAsync(companyId, id);

            if (data == null)
                return CreateResponse<object>("success", null, "register not found");

            return CreateResponse("success", data, "success");
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateProjectSkill([FromForm] string dto, [FromForm] IFormFile? file)
    {
        return await this.HandleClientOperationAsync<ProjectSkillDetailDTO>(dto, file, async (model) =>
        {
            await _service.Create(model);
            return NoContent();
        });
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateProjectSkill(int id, [FromForm] string dto, [FromForm] IFormFile? file)
    {
        return await this.HandleClientOperationAsync<ProjectSkillDetailDTO>(dto, file, async (model) =>
        {
            await _service.Update(id, model);
            return NoContent();
        });
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteProjectSkill(int id)
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
