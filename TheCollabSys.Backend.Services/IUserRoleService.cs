using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IUserRoleService
{
    Task<AspNetUserRole> AddUserRoleAsync(AspNetUserRole entity);
    Task<UserRoleDTO?> GetUserRoleByUserName(string username);
    Task<UserRoleDTO?> UpdateUserRoleByUserName(string username, string newRoleId);
}
