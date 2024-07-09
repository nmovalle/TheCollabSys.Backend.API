using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Data;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace TheCollabSys.Backend.Services;

public class SubMenuService : ISubMenuService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<SubMenuDTO, DdSubMenu> _mapperService;

    public SubMenuService(
        IUnitOfWork unitOfWork,
        IMapperService<SubMenuDTO, DdSubMenu> mapperService
        )
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }

    public IAsyncEnumerable<SubMenuDTO> GetAll()
    {
        var data = _unitOfWork.SubMenuRepository.GetAllQueryable()
            .Select(c => new SubMenuDTO
            {
                SubMenuId = c.SubMenuId,
                MenuId = c.MenuId,
                MenuName = c.Menu.MenuName,
                SubMenuName = c.SubMenuName,
                Description = c.Description,
                Icon = c.Icon,
                RouterLink = c.RouterLink
            })
            .AsAsyncEnumerable();

        return data;
    }

    public async Task<SubMenuDTO?> GetByIdAsync(int id)
    {
        var resp = await _unitOfWork.SubMenuRepository.GetAllQueryable()
            .Where(c => c.SubMenuId == id)
            .Select(c => new SubMenuDTO
            {
                SubMenuId = c.SubMenuId,
                MenuId = c.MenuId,
                MenuName = c.Menu.MenuName,
                SubMenuName = c.SubMenuName,
                Description = c.Description,
                Icon = c.Icon,
                RouterLink = c.RouterLink
            })
            .FirstOrDefaultAsync();

        return resp;
    }

    public IAsyncEnumerable<SubMenuDTO> GetByMenuIdAsync(int menuId)
    {
        var data = _unitOfWork.SubMenuRepository.GetAllQueryable()
            .Where(c => c.MenuId == menuId)
            .Select(c => new SubMenuDTO
            {
                SubMenuId = c.SubMenuId,
                MenuId = c.MenuId,
                MenuName = c.Menu.MenuName,
                SubMenuName = c.SubMenuName,
                Description = c.Description,
                Icon = c.Icon,
                RouterLink = c.RouterLink
            })
            .AsAsyncEnumerable();

        return data;
    }

    public async Task<DdSubMenu> Create(DdSubMenu entity)
    {
        //entity.DateCreated = DateTime.Now;
        _unitOfWork.SubMenuRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task Update(int id, SubMenuDTO dto)
    {
        var existing = await _unitOfWork.SubMenuRepository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("submenu not found");

        //dto.DateUpdate = DateTime.Now;
        //var excludeProperties = new List<string> { "DateCreated" };
        _mapperService.Map(dto, existing);

        _unitOfWork.SubMenuRepository.Update(existing);

        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _unitOfWork.SubMenuRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentException("submenu not found");
        }

        _unitOfWork.SubMenuRepository.Remove(entity);
        await _unitOfWork.CompleteAsync();
    }
}
