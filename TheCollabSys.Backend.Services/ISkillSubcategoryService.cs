using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface ISkillSubcategoryService
{
    IAsyncEnumerable<SkillSubcategoryDTO> GetAll();
    Task<SkillSubcategoryDTO?> GetByIdAsync(int id);
    Task<DdSkillSubcategory> Create(DdSkillSubcategory entity);
    Task Update(int id, SkillSubcategoryDTO dto);
    Task Delete(int id);
}
