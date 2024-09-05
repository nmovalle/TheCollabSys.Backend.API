using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class InvitationRepository : Repository<DdInvitation>, IInvitationRepository
{
    public InvitationRepository(TheCollabsysContext context) : base(context)
    {
    }

    public async Task<DdInvitation?> GetByToken(Guid token)
    {
        return await _context.DD_Invitations
        .FirstOrDefaultAsync(x => x.Token == token);
    }
    public async Task<DdInvitation?> GetByEmail(string email)
    {
        return await _context.DD_Invitations
        .FirstOrDefaultAsync(x => x.Email == email);
    }
}
