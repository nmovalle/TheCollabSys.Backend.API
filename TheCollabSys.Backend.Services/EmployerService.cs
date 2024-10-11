using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace TheCollabSys.Backend.Services;

public class EmployerService : IEmployerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<EmployerDTO, DdEmployer> _mapperService;

    public EmployerService(
        IUnitOfWork unitOfWork,
        IMapperService<EmployerDTO, DdEmployer> mapperService
        )
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }
    public IAsyncEnumerable<EmployerDTO> GetAll(int companyId)
    {
        var data = _unitOfWork.EmployerRepository.GetAllQueryable()
            .Where(c => c.CompanyId == companyId)
            .Select(c => new EmployerDTO
            {
                EmployerId = c.EmployerId,
                EmployerName = c.EmployerName,
                Address = c.Address,
                Phone = c.Phone,
                Image = c.Image,
                Filetype = c.Filetype,
                DateCreated = c.DateCreated,
                DateUpdate = c.DateUpdate,
                Active = c.Active
            })
            .AsAsyncEnumerable();

        return data;
    }

    public async Task<EmployerDTO> GetOldestEmployerAsync(int companyId)
    {
        var oldestEmployer = await _unitOfWork.EmployerRepository.GetAllQueryable()
            .Where(c => c.CompanyId == companyId)
            .OrderBy(c => c.DateCreated)
            .Select(c => new EmployerDTO
            {
                EmployerId = c.EmployerId,
                EmployerName = c.EmployerName,
                Address = c.Address,
                Phone = c.Phone,
                Image = c.Image,
                Filetype = c.Filetype,
                DateCreated = c.DateCreated,
                DateUpdate = c.DateUpdate,
                Active = c.Active
            })
            .FirstOrDefaultAsync();

        return oldestEmployer;
    }


    public async Task<EmployerDTO?> GetByIdAsync(int companyId, int id)
    {
        var resp = await _unitOfWork.EmployerRepository.GetAllQueryable()
            .Where(c => c.CompanyId == companyId && c.EmployerId == id)
            .Select(c => new EmployerDTO
            {
                EmployerId = c.EmployerId,
                EmployerName = c.EmployerName,
                Address = c.Address,
                Phone = c.Phone,
                Image = c.Image,
                Filetype = c.Filetype,
                DateCreated = c.DateCreated,
                DateUpdate = c.DateUpdate,
                Active = c.Active
            })
            .FirstOrDefaultAsync();

        return resp;
    }

    public async Task<DdEmployer> Create(DdEmployer entity)
    {
        entity.DateCreated = DateTime.Now;
        _unitOfWork.EmployerRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task Update(int id, EmployerDTO dto)
    {
        var existing = await _unitOfWork.EmployerRepository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("employer not found");

        dto.DateUpdate = DateTime.Now;

        var excludeProperties = new List<string> { "DateCreated" };
        _mapperService.Map(dto, existing, excludeProperties);

        _unitOfWork.EmployerRepository.Update(existing);

        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _unitOfWork.EmployerRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentException("employeer not found");
        }

        _unitOfWork.EmployerRepository.Remove(entity);
        await _unitOfWork.CompleteAsync();
    }
}
