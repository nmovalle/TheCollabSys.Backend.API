using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface ITokenService
{
    public Task<AspNetUserToken?> GetTokenAsync(string refreshToken);
    public Task InsertOrUpdateTokenAsync(string userId, AspNetUserTokensDTO dto);
    public Task<AspNetUserToken> InsertTokenAsync(AspNetUserToken token);
    public Task UpdateToken(string userId, AspNetUserTokensDTO token);
}
