using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IMenuService
{
    IAsyncEnumerable<MenuDTO> GetAll();
    Task<MenuDTO?> GetByIdAsync(int id);
    Task<DdMenu> Create(DdMenu entity);
    Task Update(int id, MenuDTO dto);
    Task Delete(int id);
}
