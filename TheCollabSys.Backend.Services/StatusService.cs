using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class StatusService : IStatusService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<StatusDTO, DdStatus> _mapperService;

    public StatusService(
        IUnitOfWork unitOfWork,
        IMapperService<StatusDTO, DdStatus> mapperService
        )
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }

    public IAsyncEnumerable<StatusDTO> GetAll()
    {
        var data = _unitOfWork.StatusRepository.GetAllQueryable()
            .Select(c => new StatusDTO
            {
                StatusId = c.StatusId,
                StatusName = c.StatusName,
                Type = c.Type
            })
            .AsAsyncEnumerable();

        return data;
    }

    public IAsyncEnumerable<StatusDTO> GetByType(string type)
    {
        var data = _unitOfWork.StatusRepository.GetAllQueryable()
            .Where(e => e.Type == type)
            .Select(c => new StatusDTO
            {
                StatusId = c.StatusId,
                StatusName = c.StatusName,
                Type = c.Type
            })
            .AsAsyncEnumerable();

        return data;
    }

    public async Task<StatusDTO?> GetByIdAsync(int id)
    {
        var resp = await _unitOfWork.StatusRepository.GetAllQueryable()
            .Where(c => c.StatusId == id)
            .Select(c => new StatusDTO
            {
                StatusId = c.StatusId,
                StatusName = c.StatusName,
                Type = c.Type
            })
            .FirstOrDefaultAsync();

        return resp;
    }

    public async Task<DdStatus> Create(DdStatus entity)
    {
        _unitOfWork.StatusRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task Update(int id, StatusDTO dto)
    {
        var existing = await _unitOfWork.StatusRepository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("status not found");

        _mapperService.Map(dto, existing);
        _unitOfWork.StatusRepository.Update(existing);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _unitOfWork.StatusRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentException("status not found");
        }

        _unitOfWork.StatusRepository.Remove(entity);
        await _unitOfWork.CompleteAsync();
    }
}
