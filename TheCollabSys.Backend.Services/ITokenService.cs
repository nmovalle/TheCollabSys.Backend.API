using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface ITokenService
{
    public Task<Token?> GetTokenAsync(string refreshToken);
    public Task<Token> InsertTokenAsync(Token token);
    public Task UpdateToken(Token token);

}
