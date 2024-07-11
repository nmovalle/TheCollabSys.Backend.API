using System.Data.Entity;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class TokenService : ITokenService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<AspNetUserTokensDTO, AspNetUserToken> _mapperService;
    public TokenService(IUnitOfWork unitOfWork, IMapperService<AspNetUserTokensDTO, AspNetUserToken> mapperService)
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }
    public async Task<AspNetUserToken?> GetTokenAsync(string refreshToken)
    {
        var resp = await _unitOfWork.TokenRepository.GetTokenFirsOrDefaultAsync(refreshToken);
            
        return resp;
    }

    public async Task InsertOrUpdateTokenAsync(string userId, AspNetUserTokensDTO dto)
    {
        var existingToken = await _unitOfWork.TokenRepository.GetTokenByUser(userId);
        if (existingToken == null)
        {
            var newToken = _mapperService.MapToDestination(dto);
            newToken.UserId = userId;
            await InsertTokenAsync(newToken);
        }
        else
        {
            await UpdateToken(userId, dto);
        }
    }

    public async Task<AspNetUserToken> InsertTokenAsync(AspNetUserToken token)
    {
        _unitOfWork.TokenRepository.Add(token);
        await _unitOfWork.CompleteAsync();
        return token;
    }

    public async Task UpdateToken(string userId, AspNetUserTokensDTO dto)
    {
        var existing = await _unitOfWork.TokenRepository.GetTokenByUser(userId);
        if (existing == null)
            throw new ArgumentException("user token not found");

        _mapperService.Map(dto, existing);
        _unitOfWork.TokenRepository.Update(existing);
        await _unitOfWork.CompleteAsync();
    }
}
