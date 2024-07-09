using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IMenuRolesService
{
    IAsyncEnumerable<MenuRoleDetailDTO> GetAll();
    IAsyncEnumerable<MenuRoleDetailDTO> GetByRole(string roleId);
    IAsyncEnumerable<MenuRoleDTO> GetMenuRoles();
    Task<MenuRoleDTO?> GetByIdAsync(int id);
    Task<DdMenuRole> Create(DdMenuRole entity);
    Task Update(int id, MenuRoleDTO dto);
    Task Delete(int id);
}
