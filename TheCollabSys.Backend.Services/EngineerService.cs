using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class EngineerService : IEngineerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<EngineerDTO, DdEngineer> _mapperService;
    public EngineerService(
        IUnitOfWork unitOfWork,
        IMapperService<EngineerDTO, DdEngineer> mapperService
        )
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }

    public IAsyncEnumerable<EngineerDTO> GetAll()
    {
        var data = _unitOfWork.EngineerRepository.GetAllQueryable()
            .Select(c => new EngineerDTO
            {
                EngineerId = c.EngineerId,
                EngineerName = c.EngineerName,
                EmployerId = c.EmployerId,
                EmployerName = c.Employer.EmployerName,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                Image = c.Image,
                Filetype = c.Filetype,
                DateCreated = c.DateCreated,
                DateUpdate = c.DateUpdate,
                IsActive = c.IsActive,
                UserId = c.UserId
            })
            .AsAsyncEnumerable();

        return data;
    }

    public async Task<EngineerDTO?> GetByIdAsync(int id)
    {
        var resp = await _unitOfWork.EngineerRepository.GetAllQueryable()
            .Where(c => c.EngineerId == id)
            .Select(c => new EngineerDTO
            {
                EngineerId = c.EngineerId,
                EngineerName = c.EngineerName,
                EmployerId = c.EmployerId,
                EmployerName = c.Employer.EmployerName,
                FirstName = c.FirstName,
                LastName = c.LastName,
                Email = c.Email,
                Phone = c.Phone,
                Image = c.Image,
                Filetype = c.Filetype,
                DateCreated = c.DateCreated,
                DateUpdate = c.DateUpdate,
                IsActive = c.IsActive,
                UserId = c.UserId
            })
            .FirstOrDefaultAsync();

        return resp;
    }

    public async Task<DdEngineer> Create(DdEngineer entity)
    {
        entity.DateCreated = DateTime.Now;
        _unitOfWork.EngineerRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task Update(int id, EngineerDTO dto)
    {
        var existing = await _unitOfWork.EngineerRepository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("engineer not found");

        dto.DateUpdate = DateTime.Now;

        var excludeProperties = new List<string> { "DateCreated" };
        _mapperService.Map(dto, existing, excludeProperties);

        _unitOfWork.EngineerRepository.Update(existing);

        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _unitOfWork.EngineerRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentException("engineer not found");
        }

        _unitOfWork.EngineerRepository.Remove(entity);
        await _unitOfWork.CompleteAsync();
    }
}
