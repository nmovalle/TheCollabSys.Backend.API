using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class MenuRolesService : IMenuRolesService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<MenuRoleDTO, DdMenuRole> _mapperService;
    public MenuRolesService(
        IUnitOfWork unitOfWork,
        IMapperService<MenuRoleDTO, DdMenuRole> mapperService
        )
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }

    public async IAsyncEnumerable<MenuRoleDetailDTO> GetAll()
    {
        var menus = await _unitOfWork.MenuRepository.GetAllQueryable()
            .Include(menu => menu.DdSubMenus)
            .ToListAsync();

        foreach (var menu in menus)
        {
            var subMenus = await _unitOfWork.SubMenuRepository.GetAllQueryable()
                .Where(subMenu => subMenu.MenuId == menu.MenuId)
                .Include(subMenu => subMenu.DdMenuRoles)
                .ToListAsync();

            var groupedSubMenus = subMenus
                .SelectMany(subMenu => subMenu.DdMenuRoles
                    .Select(menuRole => new SubMenuRoleDTO
                    {
                        label = subMenu.SubMenuName,
                        icon = subMenu.Icon,
                        routerLink = subMenu.RouterLink,
                        view = menuRole.View,
                        add = menuRole.Add,
                        edit = menuRole.Edit,
                        delete = menuRole.Delete,
                        export = menuRole.Export,
                        import = menuRole.Import,
                        roleId = menuRole.RoleId
                    }))
                .GroupBy(subMenuRole => subMenuRole.roleId);

            var menuDetails = groupedSubMenus.Select(group =>
                new MenuRoleDetailDTO
                {
                    label = menu.MenuName,
                    menuId = menu.MenuId,
                    roleId = group.Key,
                    items = group.ToList()
                })
                .OrderBy(x => x.roleId);

            foreach (var menuDetail in menuDetails)
            {
                yield return menuDetail;
            }
        }
    }

    public async IAsyncEnumerable<MenuRoleDetailDTO> GetByRole(string roleId)
    {
        var menus = await _unitOfWork.MenuRepository.GetAllQueryable()
            .Include(menu => menu.DdSubMenus)
            .ToListAsync();

        foreach (var menu in menus)
        {
            var subMenus = await _unitOfWork.SubMenuRepository.GetAllQueryable()
                .Where(subMenu => subMenu.MenuId == menu.MenuId)
                .Include(subMenu => subMenu.DdMenuRoles)
                .ToListAsync();

            var filteredSubMenus = subMenus
                .Where(subMenu => subMenu.DdMenuRoles.Any(menuRole => menuRole.RoleId == roleId))
                .ToList();

            if (filteredSubMenus.Any())
            {
                var menuDetail = new MenuRoleDetailDTO
                {
                    label = menu.MenuName,
                    menuId = menu.MenuId,
                    roleId = roleId,
                    items = filteredSubMenus.SelectMany(subMenu => subMenu.DdMenuRoles
                        .Where(menuRole => menuRole.RoleId == roleId)
                        .Select(menuRole => new SubMenuRoleDTO
                        {
                            label = subMenu.SubMenuName,
                            icon = subMenu.Icon,
                            routerLink = subMenu.RouterLink,
                            view = menuRole.View,
                            add = menuRole.Add,
                            edit = menuRole.Edit,
                            delete = menuRole.Delete,
                            export = menuRole.Export,
                            import = menuRole.Import
                        })).ToList()
                };

                yield return menuDetail;
            }
        }
    }

    public IAsyncEnumerable<MenuRoleDTO> GetMenuRoles()
    {
        var data = _unitOfWork.MenuRolesRepository.GetAllQueryable()
            .Select(c => new MenuRoleDTO
            {
                MenuRoleId = c.MenuRoleId,
                RoleId = c.RoleId,
                RoleName = c.Role.Name,
                SubMenuId = c.SubMenuId,
                SubMenuName = c.SubMenu.SubMenuName,
                View = c.View,
                Add = c.Add,
                Edit = c.Edit,
                Delete = c.Delete,
                Export = c.Export,
                Import = c.Import
            })
            .OrderBy(c => c.RoleId)
            .AsAsyncEnumerable();

        return data;
    }

    public async Task<MenuRoleDTO?> GetByIdAsync(int id)
    {
        var resp = await _unitOfWork.MenuRolesRepository.GetAllQueryable()
            .Where(c => c.MenuRoleId == id)
            .Select(c => new MenuRoleDTO
            {
                MenuRoleId = c.MenuRoleId,
                RoleId = c.RoleId,
                RoleName = c.Role.Name,
                SubMenuId = c.SubMenuId,
                SubMenuName = c.SubMenu.SubMenuName,
                View = c.View,
                Add = c.Add,
                Edit = c.Edit,
                Delete = c.Delete,
                Export = c.Export,
                Import = c.Import
            }) 
            .OrderBy(c => c.RoleId)
            .FirstOrDefaultAsync();

        return resp;
    }

    public async Task<DdMenuRole> Create(DdMenuRole entity)
    {
        _unitOfWork.MenuRolesRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task Update(int id, MenuRoleDTO dto)
    {
        var existing = await _unitOfWork.MenuRolesRepository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("menu role not found");

        _mapperService.Map(dto, existing);

        _unitOfWork.MenuRolesRepository.Update(existing);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _unitOfWork.MenuRolesRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentException("menu role not found");
        }

        _unitOfWork.MenuRolesRepository.Remove(entity);
        await _unitOfWork.CompleteAsync();
    }
}
