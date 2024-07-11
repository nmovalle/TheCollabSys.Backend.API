using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class TokenRepository : Repository<AspNetUserToken>, ITokenRepository
{
    public TokenRepository(TheCollabsysContext context) : base(context)
    {
    }

    public async Task<AspNetUserToken?> GetTokenFirsOrDefaultAsync(string refreshToken)
    {
        return await _context.AspNetUserTokens.FirstOrDefaultAsync(t => t.Value == refreshToken);
    }
    public async Task<AspNetUserToken?> GetTokenByUser(string userId)
    {
        return await _context.AspNetUserTokens.FirstOrDefaultAsync(t => t.UserId == userId);
    }
}
