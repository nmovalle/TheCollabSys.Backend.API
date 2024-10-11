using Microsoft.EntityFrameworkCore;
using System;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class DomainService : IDomainService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<DomainMasterDTO, DdDomainMaster> _mapperService;

    public DomainService(IUnitOfWork unitOfWork, IMapperService<DomainMasterDTO, DdDomainMaster> mapperService)
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }

    public IAsyncEnumerable<DomainMasterDTO> GetAll()
    {
        var data = _unitOfWork.DomainRepository.GetAllQueryable()
            .Select(c => new DomainMasterDTO
            {
                Id = c.Id,
                Domain = c.Domain,
                FullName = c.FullName,
                Active = c.Active,
                DateCreated = c.DateCreated
            })
            .AsAsyncEnumerable();

        return data;
    }

    public async Task<DomainMasterDTO?> GetByIdAsync(int id)
    {
        var resp = await _unitOfWork.DomainRepository.GetAllQueryable()
            .Where(c => c.Id == id)
            .Select(c => new DomainMasterDTO
            {
                Id = c.Id,
                Domain = c.Domain,
                FullName = c.FullName,
                Active = c.Active,
                DateCreated = c.DateCreated
            })
            .FirstOrDefaultAsync();

        return resp;
    }

    public async Task<DomainMasterDTO?> GetByDomainAsync(string domain)
    {
        var resp = await _unitOfWork.DomainRepository.GetAllQueryable()
            .Where(c => c.Domain == domain)
            .Select(c => new DomainMasterDTO
            {
                Id = c.Id,
                Domain = c.Domain,
                FullName = c.FullName,
                Active = c.Active,
                DateCreated = c.DateCreated
            })
            .FirstOrDefaultAsync();

        return resp;
    }

    public async Task<DomainMasterDTO?> GetByUsername(string username)
    {
        var resp = await _unitOfWork.DomainRepository.GetAllQueryable()
            .Join(_unitOfWork.CompanyRepository.GetAllQueryable(),
                  dm => dm.Id,
                  c => c.DomainmasterId,
                  (dm, c) => new { DomainMaster = dm, Company = c })
            .Join(_unitOfWork.UserCompanyRepository.GetAllQueryable(),
                  combined => combined.Company.CompanyId,
                  uc => uc.CompanyId,
                  (combined, uc) => new { combined.DomainMaster, combined.Company, UserCompany = uc })
            .Join(_unitOfWork.UserRepository.GetAllQueryable(),
                  combined => combined.UserCompany.UserId,
                  u => u.Id,
                  (combined, u) => new { combined.DomainMaster, combined.Company, combined.UserCompany, User = u })
            .Where(result => result.User.UserName == username)
            .Select(result => new DomainMasterDTO
            {
                Id = result.DomainMaster.Id,
                Domain = result.DomainMaster.Domain,
                FullName = result.DomainMaster.FullName,
                Active = result.DomainMaster.Active,
                DateCreated = result.DomainMaster.DateCreated
            })
            .FirstOrDefaultAsync();

        return resp;
    }

    public async Task<DdDomainMaster> Create(DdDomainMaster entity)
    {
        entity.DateCreated = DateTime.Now;
        entity.Active = true;
        _unitOfWork.DomainRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }
}
