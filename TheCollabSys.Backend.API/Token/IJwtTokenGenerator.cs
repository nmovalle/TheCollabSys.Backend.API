using TheCollabSys.Backend.Entity.Response;

namespace TheCollabSys.Backend.API.Token;

public interface IJwtTokenGenerator
{
    Task<AuthTokenResponse> GenerateToken(string username);
    Task<AuthTokenResponse> RefreshToken(string refreshToken);
}
