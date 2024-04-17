using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Interfaces;

public interface IProposalRoleRepository : IRepository<DdProposalRole>
{
    Task<ProposalRoleDTO?> GetProposalRoleByName(string name);
}
