using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface ISkillCategoryService
{
    IAsyncEnumerable<SkillCategoryDTO> GetAll();
    Task<SkillCategoryDTO?> GetByIdAsync(int id);
    Task<DdSkillCategory> Create(DdSkillCategory entity);
    Task Update(int id, SkillCategoryDTO dto);
    Task Delete(int id);
}
