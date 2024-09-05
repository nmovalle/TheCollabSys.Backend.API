using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class UserCompanyService : IUserCompanyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<UserCompanyDTO, DdUserCompany> _mapperService;

    public UserCompanyService(IUnitOfWork unitOfWork, IMapperService<UserCompanyDTO, DdUserCompany> mapperService)
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }

    public IAsyncEnumerable<UserCompanyDTO> GetAll()
    {
        var data = _unitOfWork.UserCompanyRepository.GetAllQueryable()
            .Select(c => new UserCompanyDTO
            {
                UserCompayId = c.UserCompayId,
                UserId = c.UserId,
                CompanyId = c.CompanyId
            })
            .AsAsyncEnumerable();

        return data;
    }

    public async Task<UserCompanyDTO?> GetByIdAsync(int id)
    {
        var resp = await _unitOfWork.UserCompanyRepository.GetAllQueryable()
            .Where(c => c.UserCompayId == id)
            .Select(c => new UserCompanyDTO
            {
                UserCompayId = c.UserCompayId,
                UserId = c.UserId,
                CompanyId = c.CompanyId
            })
            .FirstOrDefaultAsync();

        return resp;
    }

    public async Task<UserCompanyDTO?> GetByUserIdAsync(string userid)
    {
        var resp = await _unitOfWork.UserCompanyRepository.GetAllQueryable()
            .Where(c => c.UserId == userid)
            .Select(c => new UserCompanyDTO
            {
                UserCompayId = c.UserCompayId,
                UserId = c.UserId,
                CompanyId = c.CompanyId
            })
            .FirstOrDefaultAsync();

        return resp;
    }

    public async Task<DdUserCompany> Create(DdUserCompany entity)
    {
        _unitOfWork.UserCompanyRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task Update(int id, UserCompanyDTO dto)
    {
        var existing = await _unitOfWork.UserCompanyRepository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("user company not found");

        //dto.DateUpdate = DateTime.Now;
        //var excludeProperties = new List<string> { "DateCreated" };
        _mapperService.Map(dto, existing);

        _unitOfWork.UserCompanyRepository.Update(existing);

        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _unitOfWork.UserCompanyRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentException("user company not found");
        }

        _unitOfWork.UserCompanyRepository.Remove(entity);
        await _unitOfWork.CompleteAsync();
    }
}
