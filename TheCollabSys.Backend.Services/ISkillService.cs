using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface ISkillService
{
    IAsyncEnumerable<SkillDTO> GetAll();
    IAsyncEnumerable<SkillDTO> GetByCategories(UniqueIdsCategoriesDTO uniqueIdsCategories);
    Task<SkillDTO?> GetByIdAsync(int id);
    Task<DdSkill> Create(DdSkill entity);
    Task Update(int id, SkillDTO dto);
    Task Delete(int id);
}
