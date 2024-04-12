namespace TheCollabSys.Backend.API.Token;

public interface IJwtTokenGenerator
{
    Task<string> GenerateToken(string username);
}
