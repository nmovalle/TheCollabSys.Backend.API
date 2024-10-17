using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class UserRoleService : IUserRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<UserRoleDTO, AspNetUserRole> _mapperService;
    public UserRoleService(IUnitOfWork unitOfWork, IMapperService<UserRoleDTO, AspNetUserRole> mapperService)
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }

    public IAsyncEnumerable<UserRoleDTO> GetAll(int companyId)
    {
        var data = (from us in _unitOfWork.UserRepository.GetAllQueryable()
                    join c in _unitOfWork.UserCompanyRepository.GetAllQueryable() on us.Id equals c.UserId
                    join ur in _unitOfWork.UserRoleRepository.GetAllQueryable() on us.Id equals ur.UserId
                    join r in _unitOfWork.RoleRepository.GetAllQueryable() on ur.RoleId equals r.Id
                    where c.CompanyId == companyId
                    group new { us, ur, r } by us.Id into grouped
                    select new UserRoleDTO
                    {
                        UserId = grouped.Key,
                        Email = grouped.Select(g => g.us.Email).FirstOrDefault(),
                        RoleId = grouped.Select(g => g.ur.RoleId).FirstOrDefault(),
                        RoleName = grouped.Select(g => g.r.Name).FirstOrDefault(),
                    }).AsAsyncEnumerable();

        return data;
    }

    public async Task<UserRoleDTO?> GetByIdAsync(string id)
    {
        var data = await (from us in _unitOfWork.UserRepository.GetAllQueryable()
                          join c in _unitOfWork.UserCompanyRepository.GetAllQueryable() on us.Id equals c.UserId
                          join ur in _unitOfWork.UserRoleRepository.GetAllQueryable() on us.Id equals ur.UserId
                          join r in _unitOfWork.RoleRepository.GetAllQueryable() on ur.RoleId equals r.Id
                          where us.Id == id
                          select new UserRoleDTO
                          {
                              UserId = us.Id,
                              Email = us.Email,
                              RoleId = ur.RoleId,
                              RoleName = r.Name
                          }).FirstOrDefaultAsync();

        return data;
    }

    public async Task<AspNetUserRole> AddUserRoleAsync(AspNetUserRole entity)
    {
        _unitOfWork.UserRoleRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task<UserRoleDTO?> GetUserRoleByUserName(string username)
    {
        return await _unitOfWork.UserRoleRepository.GetUserRoleByUserName(username);
    }

    public async Task<UserRoleDTO?> UpdateUserRoleByUserName(string username, string newRoleId)
    {
        return await _unitOfWork.UserRoleRepository.UpdateUserRoleByUserName(username, newRoleId);
    }

    public async Task<AspNetUserRole> Create(AspNetUserRole entity)
    {
        //entity.DateCreated = DateTime.Now;
        _unitOfWork.UserRoleRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task Delete(string id)
    {
        var entity = await _unitOfWork.UserRoleRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentException("user role not found");
        }

        _unitOfWork.UserRoleRepository.Remove(entity);
        await _unitOfWork.CompleteAsync();
    }
}
