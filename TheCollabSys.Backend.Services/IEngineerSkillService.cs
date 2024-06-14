using TheCollabSys.Backend.Entity.DTOs;

namespace TheCollabSys.Backend.Services;

public interface IEngineerSkillService
{
    IAsyncEnumerable<EngineerSkillDetailDTO> GetAll();
    Task<EngineerSkillDetailDTO?> GetByIdAsync(int id);
    Task Create(EngineerSkillDetailDTO entity);
    Task Update(int id, EngineerSkillDetailDTO dto);
    Task Delete(int id);
}
