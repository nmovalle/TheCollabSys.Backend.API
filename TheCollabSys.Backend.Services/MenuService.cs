using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace TheCollabSys.Backend.Services;

public class MenuService : IMenuService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<MenuDTO, DdMenu> _mapperService;

    public MenuService(
        IUnitOfWork unitOfWork,
        IMapperService<MenuDTO, DdMenu> mapperService
        )
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }

    public IAsyncEnumerable<MenuDTO> GetAll()
    {
        var data = _unitOfWork.MenuRepository.GetAllQueryable()
            .Select(c => new MenuDTO
            {
                MenuId = c.MenuId,
                MenuName = c.MenuName,
                Description = c.Description
            })
            .AsAsyncEnumerable();

        return data;
    }

    public async Task<MenuDTO?> GetByIdAsync(int id)
    {
        var resp = await _unitOfWork.MenuRepository.GetAllQueryable()
            .Where(c => c.MenuId == id)
            .Select(c => new MenuDTO
            {
                MenuId = c.MenuId,
                MenuName = c.MenuName,
                Description = c.Description
            })
            .FirstOrDefaultAsync();

        return resp;
    }

    public async Task<DdMenu> Create(DdMenu entity)
    {
        //entity.DateCreated = DateTime.Now;
        _unitOfWork.MenuRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task Update(int id, MenuDTO dto)
    {
        var existing = await _unitOfWork.MenuRepository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("menu not found");

        //dto.DateUpdate = DateTime.Now;
        //var excludeProperties = new List<string> { "DateCreated" };
        _mapperService.Map(dto, existing);

        _unitOfWork.MenuRepository.Update(existing);
        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _unitOfWork.MenuRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentException("menu not found");
        }

        _unitOfWork.MenuRepository.Remove(entity);
        await _unitOfWork.CompleteAsync();
    }
}
