using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IUserRoleService
{
    IAsyncEnumerable<UserRoleDTO> GetAll(int companyId);
    Task<UserRoleDTO?> GetByIdAsync(string id);
    Task<AspNetUserRole> AddUserRoleAsync(AspNetUserRole entity);
    Task<UserRoleDTO?> GetUserRoleByUserName(string username);
    Task<UserRoleDTO?> UpdateUserRoleByUserName(string username, string newRoleId);
    Task<AspNetUserRole> Create(AspNetUserRole entity);
    Task Delete(string id);
}
