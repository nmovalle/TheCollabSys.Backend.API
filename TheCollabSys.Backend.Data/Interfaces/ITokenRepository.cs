using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Interfaces;

public interface ITokenRepository : IRepository<Token>
{
    Task<Token?> GetTokenFirsOrDefaultAsync(string refreshToken);
}
