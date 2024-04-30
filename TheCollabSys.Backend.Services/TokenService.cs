using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class TokenService : ITokenService
{
    private readonly IUnitOfWork _unitOfWork;
    public TokenService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Token?> GetTokenAsync(string refreshToken)
    {
        return await _unitOfWork.TokenRepository.GetTokenFirsOrDefaultAsync(refreshToken);
    }

    public async Task<Token> InsertTokenAsync(Token token)
    {
        _unitOfWork.TokenRepository.Add(token);
        await _unitOfWork.CompleteAsync();
        return token;
    }

    public async Task UpdateToken(Token token)
    {
        _unitOfWork.TokenRepository.Update(token);
        await _unitOfWork.CompleteAsync();
    }
}
