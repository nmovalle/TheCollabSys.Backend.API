using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using TheCollabSys.Backend.Services;
namespace TheCollabSys.Backend.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProposalRolesController : ControllerBase
{
    private readonly ILogger<ProposalRolesController> _logger;
    private readonly IProposalRoleService _proposalRoleService;
    private readonly IMapperService<ProposalRoleDTO, DdProposalRole> _mapperService;

    public ProposalRolesController(ILogger<ProposalRolesController> logger, IProposalRoleService proposalRoleService, IMapperService<ProposalRoleDTO, DdProposalRole> mapperService)
    {
        _logger = logger;
        _proposalRoleService = proposalRoleService;
        _mapperService = mapperService;
    }

    [HttpGet("GetAllProposalRolesAsync", Name = "GetAllProposalRolesAsync")]
    [ActionName(nameof(GetAllProposalRolesAsync))]
    public async Task<IActionResult> GetAllProposalRolesAsync()
    {
        return Ok(await _proposalRoleService.GetAllProposalRolesAsync());
    }

    [HttpGet("GetProposalRoleByIdAsync/{id}", Name = "GetProposalRoleByIdAsync")]
    [ActionName(nameof(GetProposalRoleByIdAsync))]
    public async Task<IActionResult> GetProposalRoleByIdAsync(int id)
    {
        var entity = await _proposalRoleService.GetProposalRoleByIdAsync(id);
        if (entity == null)
            return NotFound();

        return Ok(entity);
    }

    [HttpGet("GetProposalRoleByNameAsync/{name}", Name = "GetProposalRoleByNameAsync")]
    [ActionName(nameof(GetProposalRoleByNameAsync))]
    public async Task<IActionResult> GetProposalRoleByNameAsync(string name)
    {
        var entity = await _proposalRoleService.GetProposalRoleByNameAsync(name);
        return Ok(entity);
    }
}
