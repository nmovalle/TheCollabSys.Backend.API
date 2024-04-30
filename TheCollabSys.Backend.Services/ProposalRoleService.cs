using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class ProposalRoleService : IProposalRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<ProposalRoleDTO, DdProposalRole> _mapperService;
    public ProposalRoleService(IUnitOfWork unitOfWork, IMapperService<ProposalRoleDTO, DdProposalRole> mapperService)
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }

    public async Task<IEnumerable<ProposalRoleDTO>> GetAllProposalRolesAsync()
    {
        var roles = await _unitOfWork.ProposalRoleRepository.GetAllAsync();
        var dto = roles.Select(_mapperService.MapToSource).ToList();

        return dto;
    }

    public async Task<ProposalRoleDTO?> GetProposalRoleByIdAsync(int id)
    {
        var entity = await _unitOfWork.ProposalRoleRepository.GetByIdAsync(id);
        return _mapperService.MapToSource(entity);
    }

    public async Task<ProposalRoleDTO?> GetProposalRoleByNameAsync(string name)
    {
        return await _unitOfWork.ProposalRoleRepository.GetProposalRoleByName(name);
    }
}
