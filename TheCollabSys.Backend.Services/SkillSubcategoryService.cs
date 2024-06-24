using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class SkillSubcategoryService : ISkillSubcategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<SkillSubcategoryDTO, DdSkillSubcategory> _mapperService;
    public SkillSubcategoryService(
        IUnitOfWork unitOfWork,
        IMapperService<SkillSubcategoryDTO, DdSkillSubcategory> mapperService)
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }

    public IAsyncEnumerable<SkillSubcategoryDTO> GetAll()
    {
        var data = (from ss in _unitOfWork.SkillSubcategoryRepository.GetAllQueryable()
                    select new SkillSubcategoryDTO
                    {
                        SubcategoryId = ss.SubcategoryId,
                        SubcategoryName = ss.SubcategoryName,
                        CategoryId = ss.CategoryId,
                        CategoryName = ss.Category.CategoryName
                    })
            .AsAsyncEnumerable();

        return data;
    }

    public async Task<SkillSubcategoryDTO?> GetByIdAsync(int id)
    {
        var data = await (from ss in _unitOfWork.SkillSubcategoryRepository.GetAllQueryable()
                          where ss.SubcategoryId == id
                          select new SkillSubcategoryDTO
                          {
                              SubcategoryId = ss.SubcategoryId,
                              SubcategoryName = ss.SubcategoryName,
                              CategoryId = ss.CategoryId,
                              CategoryName = ss.Category.CategoryName
                          })
            .FirstOrDefaultAsync();

        return data;
    }

    public async Task<DdSkillSubcategory> Create(DdSkillSubcategory entity)
    {
        _unitOfWork.SkillSubcategoryRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task Update(int id, SkillSubcategoryDTO dto)
    {
        var existing = await _unitOfWork.SkillSubcategoryRepository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("skill category not found");

        _mapperService.Map(dto, existing, null);

        _unitOfWork.SkillSubcategoryRepository.Update(existing);

        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _unitOfWork.SkillSubcategoryRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentException("skill category not found");
        }

        _unitOfWork.SkillSubcategoryRepository.Remove(entity);
        await _unitOfWork.CompleteAsync();
    }
}
