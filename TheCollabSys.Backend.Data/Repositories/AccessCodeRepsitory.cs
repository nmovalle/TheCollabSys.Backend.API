using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class AccessCodeRepsitory : Repository<DdAccessCode>, IAccessCodeRepsitory
{
    public AccessCodeRepsitory(TheCollabsysContext context) : base(context)
    {
    }

    public async Task<DdAccessCode?> GetByEmail(string email)
    {
        return await _context.DD_AccessCode
        .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<DdAccessCode?> GetByAccessCodeEmail(string accessCode, string email)
    {
        return await _context.DD_AccessCode
        .FirstOrDefaultAsync(x => x.AccessCode == accessCode && x.Email == email);
    }
}
