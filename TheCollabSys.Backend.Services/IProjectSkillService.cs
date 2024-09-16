using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IProjectSkillService
{
    IAsyncEnumerable<ProjectSkillDetailDTO> GetAll(int companyId);
    Task<ProjectSkillDetailDTO?> GetByIdAsync(int companyId, int id);
    Task Create(ProjectSkillDetailDTO entity);
    Task Update(int id, ProjectSkillDetailDTO dto);
    Task Delete(int id);
}
