using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class WireListRepository : Repository<DdWireList>, IWireListRepository
{
    public WireListRepository(TheCollabsysContext context) : base(context)
    {
    }
    public async Task<DdWireList?> GetByEmail(string email)
    {
        return await _context.DD_WireList
        .FirstOrDefaultAsync(x => x.Email == email);
    }
}
