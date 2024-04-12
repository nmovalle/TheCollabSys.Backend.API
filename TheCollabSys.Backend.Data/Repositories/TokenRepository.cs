using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class TokenRepository : Repository<Token>, ITokenRepository
{
    public TokenRepository(TheCollabsysContext context) : base(context)
    {
    }

    public async Task<Token?> GetTokenFirsOrDefaultAsync(string refreshToken)
    {
        return await _context.Token.FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);
    }
}
