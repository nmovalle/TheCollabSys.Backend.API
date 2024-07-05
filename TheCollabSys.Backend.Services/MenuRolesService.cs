using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;

namespace TheCollabSys.Backend.Services;

public class MenuRolesService : IMenuRolesService
{
    private readonly IUnitOfWork _unitOfWork;
    public MenuRolesService(
        IUnitOfWork unitOfWork
        )
    {
        _unitOfWork = unitOfWork;
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
}
