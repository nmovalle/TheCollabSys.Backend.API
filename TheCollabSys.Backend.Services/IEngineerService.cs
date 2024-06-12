using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IEngineerService
{
    IAsyncEnumerable<EngineerDTO> GetAll();
    Task<EngineerDTO?> GetByIdAsync(int id);
    Task<DdEngineer> Create(DdEngineer entity);
    Task Update(int id, EngineerDTO dto);
    Task Delete(int id);
}
