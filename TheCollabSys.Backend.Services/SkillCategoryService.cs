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
