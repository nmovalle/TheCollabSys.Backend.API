using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.API.Extensions;
using TheCollabSys.Backend.API.Filters;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Controllers;

//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(GlobalExceptionFilter))]
[ServiceFilter(typeof(ModelStateFilter))]
public class CompanyController : BaseController
{
    private readonly ICompanyService _service;
    private readonly IMapperService<CompanyDTO, DdCompany> _mapper;
    public CompanyController(
        ICompanyService service,
        IMapperService<CompanyDTO, DdCompany> mapperService
        )
    {
        _service = service;
        _mapper = mapperService;
    }

    [HttpGet("GetAll/{companyId}")]
    public async Task<IActionResult> GetAll(int companyId)
    {
        return await ExecuteAsync(async () =>
        {
            var data = await _service.GetAll(companyId).ToListAsync();

            if (data.Any())
                return CreateResponse("success", data, "success");

            return CreateNotFoundResponse<object>(null, "Registers not founds");
        });
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        return await ExecuteAsync(async () =>
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
                return CreateNotFoundResponse<object>(null, "register not found");

            return CreateResponse("success", data, "success");
        });
    }

    [HttpGet]
    [Route("GetByDomain/{domain}")]
    public async Task<IActionResult> GetByDomain(string domain)
    {
        return await ExecuteAsync(async () =>
        {
            var data = await _service.GetByIdDomainAsync(domain);

            if (data == null)
                return CreateNotFoundResponse<object>(null, "registers not found");

            return CreateResponse("success", data, "success");
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] string dto, [FromForm] IFormFile? file)
    {
        return await this.HandleClientOperationAsync<CompanyDTO>(dto, file, async (model) =>
        {
            var entity = _mapper.MapToDestination(model);
            var savedEntity = await _service.Create(entity);
            return CreateResponse("success", savedEntity, "success");
        });
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] string dto, [FromForm] IFormFile? file)
    {
        return await this.HandleClientOperationAsync<CompanyDTO>(dto, file, async (model) =>
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null) return NotFound();

            await _service.Update(id, model);
            return NoContent();
        });
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(int id)
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
