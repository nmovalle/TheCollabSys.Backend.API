using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.API.Extensions;
using TheCollabSys.Backend.API.Filters;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Controllers;

//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] //vm comentar solo para probar para no requerir Token
[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(GlobalExceptionFilter))]
[ServiceFilter(typeof(ModelStateFilter))]
[ServiceFilter(typeof(UserIdFilter))]
public class EngineersController : BaseController
{
    private readonly IEngineerService _service;
    private readonly IEngineerSkillService _serviceSkills;
    private readonly IMapperService<EngineerDTO, DdEngineer> _mapper;
    public EngineersController(
        IEngineerService service,
        IEngineerSkillService serviceSkills,
        IMapperService<EngineerDTO, DdEngineer> mapperService
        )
    {
        _service = service;
        _serviceSkills = serviceSkills;
        _mapper = mapperService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEngineers([FromQuery] string email = null) //[FromQuery] para aceptar el parámetro opcional engineerId en la URL (/api/engineers?email=valerio@gmail.com VMP
    {
        return await ExecuteWithCompanyIdAsync(async (companyId) =>
        {
            var data = await _service.GetAll(companyId, email).ToListAsync(); //agrego filtro opcional engineerId

            if (data.Any())
                return CreateResponse("success", data, "success");

            return CreateNotFoundResponse<object>(null, "Registers not founds");
        });
    }


    [HttpGet]
    [Route("GetDetail")]
    public async Task<IActionResult> GetDetail([FromQuery] string email = null)
    {
        return await ExecuteWithCompanyIdAsync(async (companyId) =>
        {
            var data = await _service.GetDetail(companyId, email).ToListAsync();

            if (data.Any())
                return CreateResponse("success", data, "success");

            return CreateNotFoundResponse<object>(null, "Registers not founds");
        });
    }

    [HttpGet]
    [Route("GetEngineersByProjectSkills/{projectId}")]
    public async Task<IActionResult> GetEngineersByProjectSkills(int projectId)
    {
        return await ExecuteWithCompanyIdAsync(async (companyId) =>
        {
            var data = await _service.GetEngineersByProjectSkillsAsync(companyId, projectId).ToListAsync();

            if (data == null)
                return CreateNotFoundResponse<object>(null, "register not found");

            return CreateResponse("success", data, "success");
        });
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetEngineerById(int id)
    {
        return await ExecuteWithCompanyIdAsync(async (companyId) =>
        {
            var data = await _service.GetByIdAsync(companyId, id);

            if (data == null)
                return CreateNotFoundResponse<object>(null, "register not found");

            return CreateResponse("success", data, "success");
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateEngineer([FromForm] string dto, [FromForm] IFormFile? file)
    {
        return await this.HandleClientOperationAsync<EngineerDTO>(dto, file, async (model) =>
        {
            var entity = _mapper.MapToDestination(model);
            var savedEntity = await _service.Create(entity);
            return CreateResponse("success", savedEntity, "success");
        });
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateEngineer(int id, [FromForm] string dto, [FromForm] IFormFile? file)
    {
        return await this.HandleClientOperationAsync<EngineerDTO>(dto, file, async (model) =>
        {
            if (model.CompanyId == null) return NotFound("Company Id is missing.");

            var existing = await _service.GetByIdAsync((int)model.CompanyId, id);
            if (existing == null)
                return CreateNotFoundResponse<object>(null, "register not found");

            var savedEntity = await _service.Update(id, model);
            return CreateResponse("success", savedEntity, "success");
        });
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteEngineer(int id)
    {
        try
        {
            await _serviceSkills.Delete(id);
            await _service.Delete(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return CreateNotFoundResponse<object>(null, "register not found");
        }
    }
}
