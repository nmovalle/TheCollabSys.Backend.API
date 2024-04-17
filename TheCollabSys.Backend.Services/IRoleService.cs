using TheCollabSys.Backend.Entity.DTOs;

namespace TheCollabSys.Backend.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleDTO>> GetAllRolesAsync();
    Task<RoleDTO?> GetRoleByIdAsync(string id);
    Task<RoleDTO?> GetRoleByNameAsync(string name);
}
