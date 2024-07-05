using TheCollabSys.Backend.Entity.DTOs;

namespace TheCollabSys.Backend.Services;

public interface IMenuRolesService
{
    IAsyncEnumerable<MenuRoleDetailDTO> GetAll();
    IAsyncEnumerable<MenuRoleDetailDTO> GetByRole(string roleId);
}
