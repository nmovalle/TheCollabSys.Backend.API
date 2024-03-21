using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class ClientRepository : Repository<DD_Clients>, IClientRepository
{
    public ClientRepository(TheCollabsysContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DD_Clients>> GetClientsByNameAsync(string name)
    {
        return await _context.DD_Clients
            .Where(c => c.ClientName.Contains(name))
            .ToListAsync();
    }
}
