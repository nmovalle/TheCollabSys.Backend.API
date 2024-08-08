using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class CompanyService : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<CompanyDTO, DdCompany> _mapperService;

    public CompanyService(IUnitOfWork unitOfWork, IMapperService<CompanyDTO, DdCompany> mapperService)
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }

    public IAsyncEnumerable<CompanyDTO> GetAll()
    {
        var data = _unitOfWork.CompanyRepository.GetAllQueryable()
            .Select(c => new CompanyDTO
            {
                CompanyId = c.CompanyId,
                DomainmasterId = c.DomainmasterId,
                FullName = c.FullName,
                Address = c.Address,
                Zipcode = c.Zipcode,
                Phone = c.Phone,
                Logo = c.Logo,
                FileType = c.FileType,
                Active = c.Active
            })
            .AsAsyncEnumerable();

        return data;
    }

    public async Task<CompanyDTO?> GetByIdAsync(int id)
    {
        var resp = await _unitOfWork.CompanyRepository.GetAllQueryable()
            .Where(c => c.CompanyId == id)
            .Select(c => new CompanyDTO
            {
                CompanyId = c.CompanyId,
                DomainmasterId = c.DomainmasterId,
                FullName = c.FullName,
                Address = c.Address,
                Zipcode = c.Zipcode,
                Phone = c.Phone,
                Logo = c.Logo,
                FileType = c.FileType,
                Active = c.Active
            })
            .FirstOrDefaultAsync();

        return resp;
    }

    public async Task<DdCompany> Create(DdCompany entity)
    {
        _unitOfWork.CompanyRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task Update(int id, CompanyDTO dto)
    {
        var existing = await _unitOfWork.CompanyRepository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("company not found");

        //dto.DateUpdate = DateTime.Now;
        //var excludeProperties = new List<string> { "DateCreated" };
        _mapperService.Map(dto, existing);

        _unitOfWork.CompanyRepository.Update(existing);

        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _unitOfWork.CompanyRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentException("company not found");
        }

        _unitOfWork.CompanyRepository.Remove(entity);
        await _unitOfWork.CompleteAsync();
    }
}
