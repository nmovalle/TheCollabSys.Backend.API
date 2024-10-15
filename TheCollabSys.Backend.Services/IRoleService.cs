using TheCollabSys.Backend.Entity.DTOs;

namespace TheCollabSys.Backend.Services;

public interface IRoleService
{
    IAsyncEnumerable<RoleDTO> GetAll();
    Task<RoleDTO?> GetByIdAsync(string id);
    Task<RoleDTO?> GetRoleByNameAsync(string name);
}
