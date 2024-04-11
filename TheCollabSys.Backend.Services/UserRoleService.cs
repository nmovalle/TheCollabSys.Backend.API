using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class UserRoleService : IUserRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    public UserRoleService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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
}
