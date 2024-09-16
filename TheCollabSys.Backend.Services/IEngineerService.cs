using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IEngineerService
{
    IAsyncEnumerable<EngineerDTO> GetAll(int companyId);
    IAsyncEnumerable<EngineerDTO> GetEngineersByProjectSkillsAsync(int companyId, int projectId);
    Task<EngineerDTO?> GetByIdAsync(int companyId, int id);
    Task<DdEngineer> Create(DdEngineer entity);
    Task Update(int id, EngineerDTO dto);
    Task Delete(int id);
}
