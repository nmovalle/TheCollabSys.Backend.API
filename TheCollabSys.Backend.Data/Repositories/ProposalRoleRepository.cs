using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class ProposalRoleRepository : Repository<DdProposalRole>, IProposalRoleRepository
{
    public ProposalRoleRepository(TheCollabsysContext context) : base(context)
    {
    }

    public async Task<ProposalRoleDTO?> GetProposalRoleByName(string name)
    {
        return await _context.DD_ProposalRoles
            .Select(r => new ProposalRoleDTO
            {
                ProposalId = r.ProposalId,
                ProposalRoles = r.ProposalRoles,
                RoleId = r.RoleId,
                ProposalName = r.ProposalName,
                Description = r.Description,
            })
            .FirstOrDefaultAsync(r => r.ProposalName == name);
    }
}
