using TheCollabSys.Backend.Entity.DTOs;

namespace TheCollabSys.Backend.Services;

public interface IEngineerSkillService
{
    IAsyncEnumerable<EngineerSkillDetailDTO> GetAll(int companyId);
    Task<EngineerSkillDetailDTO?> GetByIdAsync(int companyId, int id);
    Task Create(EngineerSkillDetailDTO entity);
    Task Update(int id, EngineerSkillDetailDTO dto);
    Task Delete(int id);
}
