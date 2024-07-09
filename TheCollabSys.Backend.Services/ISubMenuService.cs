using System.Threading.Tasks;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface ISubMenuService
{
    IAsyncEnumerable<SubMenuDTO> GetAll();
    Task<SubMenuDTO?> GetByIdAsync(int id);
    IAsyncEnumerable<SubMenuDTO> GetByMenuIdAsync(int menuId);
    Task<DdSubMenu> Create(DdSubMenu entity);
    Task Update(int id, SubMenuDTO dto);
    Task Delete(int id);
}
