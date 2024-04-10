using Microsoft.AspNetCore.Http;
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
public class DomainController : ControllerBase
{
    private readonly ILogger<DomainController> _logger;
    private readonly IDomainService _domainService;

    public DomainController(ILogger<DomainController> logger, IDomainService domainService)
    {
        _logger = logger;
        _domainService = domainService;
    }

    [HttpGet("GetAllDomainMastersAsync", Name = "GetAllDomainMastersAsync")]
    [ActionName(nameof(GetAllDomainMastersAsync))]
    public async Task<IActionResult> GetAllDomainMastersAsync()
    {
        return Ok(await _domainService.GetAllDomainMastersAsync());
    }

    [HttpGet("GetDomainMasterByIdAsync/{id}", Name = "GetDomainMasterByIdAsync")]
    [ActionName(nameof(GetDomainMasterByIdAsync))]
    public async Task<IActionResult> GetDomainMasterByIdAsync(int id)
    {
        var client = await _domainService.GetDomainMasterById(id);
        if (client == null)
            return NotFound();

        return Ok(client);
    }

    [HttpGet("GetDomainMasterByDomainAsync/{domain}", Name = "GetDomainMasterByDomainAsync")]
    [ActionName(nameof(GetDomainMasterByDomainAsync))]
    public async Task<IActionResult> GetDomainMasterByDomainAsync(string domain)
    {
        var domainMaster = await _domainService.GetDomainMasterByDomain(domain);
        return Ok(domainMaster);
    }

    [HttpPost]
    [ActionName(nameof(AddDomainMasterAsync))]
    public async Task<IActionResult> AddDomainMasterAsync(DdDomainMaster model)
    {
        var savedDomain  = await _domainService.AddDomainMaster(model);

        return CreatedAtAction(nameof(GetDomainMasterByIdAsync), new { id = savedDomain.Id }, savedDomain);
    }
}
