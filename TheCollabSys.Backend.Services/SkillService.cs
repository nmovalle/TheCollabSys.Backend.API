using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Data;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using Microsoft.EntityFrameworkCore;

namespace TheCollabSys.Backend.Services;

public class SkillService : ISkillService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<SkillDTO, DdSkill> _mapperService;
    private readonly TheCollabsysContext _context;

    public SkillService(
        IUnitOfWork unitOfWork,
        IMapperService<SkillDTO, DdSkill> mapperService,
        TheCollabsysContext context
        )
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
        _context = context;
    }
    public IAsyncEnumerable<SkillDTO> GetAll()
    {
        var data = _unitOfWork.SkillRepository.GetAllQueryable()
            .Select(c => new SkillDTO
            {
                SkillId = c.SkillId,
                SkillName = c.SkillName,
                CategoryId = c.CategoryId,
                CategoryName = c.Category.CategoryName,
                SubcategoryId = c.SubcategoryId,
                SubcategoryName = c.Subcategory.SubcategoryName
            })
            .AsAsyncEnumerable();

        return data;
    }

    public async Task<SkillDTO?> GetByIdAsync(int id)
    {
        var resp = await _unitOfWork.SkillRepository.GetAllQueryable()
            .Where(c => c.SkillId == id)
            .Select(c => new SkillDTO
            {
                SkillId = c.SkillId,
                SkillName = c.SkillName,
                CategoryId = c.CategoryId,
                CategoryName = c.Category.CategoryName,
                SubcategoryId = c.SubcategoryId,
                SubcategoryName = c.Subcategory.SubcategoryName
            })
            .FirstOrDefaultAsync();

        return resp;
    }

    public async Task<DdSkill> Create(DdSkill entity)
    {
        //entity.DateCreated = DateTime.Now;
        _unitOfWork.SkillRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task Update(int id, SkillDTO dto)
    {
        var existing = await _unitOfWork.SkillRepository.GetByIdAsync(id);
        if (existing == null)
            throw new ArgumentException("skill not found");

        //dto.DateUpdate = DateTime.Now;
        //var excludeProperties = new List<string> { "DateCreated" };
        _mapperService.Map(dto, existing);

        _unitOfWork.SkillRepository.Update(existing);

        await _unitOfWork.CompleteAsync();
    }

    public async Task Delete(int id)
    {
        var entity = await _unitOfWork.SkillRepository.GetByIdAsync(id);
        if (entity == null)
        {
            throw new ArgumentException("skill not found");
        }

        _unitOfWork.SkillRepository.Remove(entity);
        await _unitOfWork.CompleteAsync();
    }
}
