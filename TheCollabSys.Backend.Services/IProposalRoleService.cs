using TheCollabSys.Backend.Entity.DTOs;

namespace TheCollabSys.Backend.Services;

public interface IProposalRoleService
{
    Task<IEnumerable<ProposalRoleDTO>> GetAllProposalRolesAsync();
    Task<ProposalRoleDTO?> GetProposalRoleByIdAsync(int id);
    Task<ProposalRoleDTO?> GetProposalRoleByNameAsync(string name);
}
