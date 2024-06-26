﻿using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class ClientRepository : Repository<DdClient>, IClientRepository
{
    public ClientRepository(TheCollabsysContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DdClient>> GetClientsByNameAsync(string name)
    {
        return await _context.DD_Clients
            .Where(c => c.ClientName.Contains(name))
            .ToListAsync();
    }
}
