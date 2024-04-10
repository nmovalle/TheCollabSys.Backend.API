using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class DomainRepository : Repository<DdDomainMaster>, IDomainRepository
{
    public DomainRepository(TheCollabsysContext context) : base(context)
    {
    }

    public async Task<DdDomainMaster?> GetDomainMasterByDomain(string domain)
    {
        return await _context.DD_DomainMaster
            .FirstOrDefaultAsync(c => c.Domain == domain);
    }
}
