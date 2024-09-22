using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IStatusService
{
    IAsyncEnumerable<StatusDTO> GetAll();
    IAsyncEnumerable<StatusDTO> GetByType(string type);
    Task<StatusDTO?> GetByIdAsync(int id);
    Task<DdStatus> Create(DdStatus entity);
    Task Update(int id, StatusDTO dto);
    Task Delete(int id);
}
