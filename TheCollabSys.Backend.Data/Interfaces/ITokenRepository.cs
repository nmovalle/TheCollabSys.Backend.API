using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Interfaces;

public interface ITokenRepository : IRepository<AspNetUserToken>
{
    Task<AspNetUserToken?> GetTokenFirsOrDefaultAsync(string refreshToken);
    Task<AspNetUserToken?> GetTokenByUser(string userId);
}
