using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Data;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace TheCollabSys.Backend.Services;

public class EmployerService : IEmployerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<EmployerDTO, DdEmployer> _mapperService;
    private readonly TheCollabsysContext _context;

    public EmployerService(
        IUnitOfWork unitOfWork,
        IMapperService<EmployerDTO, DdEmployer> mapperService,
        TheCollabsysContext context
        )
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
        _context = context;
    }
    public IAsyncEnumerable<EmployerDTO> GetAll()
    {
        var data = _unitOfWork.EmployerRepository.GetAllQueryable()
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

    public async Task<EmployerDTO?> GetByIdAsync(int id)
    {
        var resp = await _unitOfWork.EmployerRepository.GetAllQueryable()
            .Where(c => c.EmployerId == id)
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
