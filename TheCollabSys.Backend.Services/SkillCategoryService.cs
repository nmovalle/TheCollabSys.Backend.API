using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class SkillCategoryService : ISkillCategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<SkillCategoryDTO, DdSkillCategory> _mapperService;
    public SkillCategoryService(
        IUnitOfWork unitOfWork,
        IMapperService<SkillCategoryDTO, DdSkillCategory> mapperService)
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }

    public IAsyncEnumerable<SkillCategoryDTO> GetAll()
    {
        var data = (from ss in _unitOfWork.SkillCategoryRepository.GetAllQueryable()
                    select new SkillCategoryDTO
                    {
                        CategoryId = ss.CategoryId,
                        CategoryName = ss.CategoryName
                    })
            .AsAsyncEnumerable();

        return data;
    }

    public async Task<SkillCategoryDTO?> GetByIdAsync(int id)
    {
        var data = await (from ss in _unitOfWork.SkillCategoryRepository.GetAllQueryable()
                    where ss.CategoryId == id
                    select new SkillCategoryDTO
                    {
                        CategoryId = ss.CategoryId,
                        CategoryName = ss.CategoryName
                    })
            .FirstOrDefaultAsync();

        return data;
    }

    private string GenerateKey(ref int counter)
    {
        return (counter++).ToString();
    }

    public async IAsyncEnumerable<SkillCategoriesDetailDTO> GetAllWithCategories()
    {
        var categories = await _unitOfWork.SkillCategoryRepository
            .GetAllQueryable()
            .Include(c => c.DdSkillSubcategories)
            .ToListAsync();

        int parentKeyCounter = 0;

        foreach (var category in categories)
        {
            var children = category.DdSkillSubcategories.Select((subcategory, index) => new SkillCategoriesDetailDTO
            {
                key = $"{parentKeyCounter}-{index}",
                label = subcategory.SubcategoryName,
                data = $"categoryId: {category.CategoryId}, subcategoryId: {subcategory.SubcategoryId}",
                icon = "pi pi-fw pi-cog",
                children = null
            }).ToList();

            yield return new SkillCategoriesDetailDTO
            {
                key = $"{parentKeyCounter}",
                label = category.CategoryName,
                data = $"categoryId: {category.CategoryId}",
                icon = "pi pi-fw pi-inbox",
                children = children.Any() ? children : null
            };

            parentKeyCounter++;
        }
    }

    public async Task<DdSkillCategory> Create(DdSkillCategory entity)
    {
        _unitOfWork.SkillCategoryRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task Update(int id, SkillCategoryDTO dto)
    {
        var existing = await _unitOfWork.SkillCategoryRepository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("skill category not found");

        _mapperService.Map(dto, existing, null);

        _unitOfWork.SkillCategoryRepository.Update(existing);

        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _unitOfWork.SkillCategoryRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentException("skill category not found");
        }

        _unitOfWork.SkillCategoryRepository.Remove(entity);
        await _unitOfWork.CompleteAsync();
    }
}
