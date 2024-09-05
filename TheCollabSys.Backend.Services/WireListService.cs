using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class WireListService : IWireListService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<WireListDTO, DdWireList> _mapperService;

    public WireListService(IUnitOfWork unitOfWork, IMapperService<WireListDTO, DdWireList> mapperService)
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }

    public IAsyncEnumerable<WireListDTO> GetAll()
    {
        var data = _unitOfWork.WireListRepository.GetAllQueryable()
            .Select(c => new WireListDTO
            {
                Id = c.Id,
                Email = c.Email,
                Domain = c.Domain,
                IsExternal = c.IsExternal,
                RoleId = c.RoleId,
                IsBlackList = c.IsBlackList
            })
            .AsAsyncEnumerable();

        return data;
    }

    public async Task<WireListDTO?> GetByIdAsync(int id)
    {
        var resp = await _unitOfWork.WireListRepository.GetAllQueryable()
            .Where(c => c.Id == id)
            .Select(c => new WireListDTO
            {
                Id = c.Id,
                Email = c.Email,
                Domain = c.Domain,
                IsExternal = c.IsExternal,
                RoleId = c.RoleId,
                IsBlackList = c.IsBlackList
            })
            .FirstOrDefaultAsync();

        return resp;
    }

    public async Task<DdWireList> Create(DdWireList entity)
    {
        _unitOfWork.WireListRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task Update(int id, WireListDTO dto)
    {
        var existing = await _unitOfWork.WireListRepository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("invitation not found");

        //dto.DateUpdate = DateTime.Now;
        //var excludeProperties = new List<string> { "DateCreated" };
        _mapperService.Map(dto, existing);

        _unitOfWork.WireListRepository.Update(existing);

        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _unitOfWork.WireListRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentException("invitation not found");
        }

        _unitOfWork.WireListRepository.Remove(entity);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<WireListDTO?> GetByEmail(string email)
    {
        var entity = await _unitOfWork.WireListRepository.GetByEmail(email);
        if (entity == null)
            return null;

        var tokenDTO = _mapperService.MapToSource(entity);
        return tokenDTO;
    }
}