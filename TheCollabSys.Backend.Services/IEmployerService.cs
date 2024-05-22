using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IEmployerService
{
    IAsyncEnumerable<EmployerDTO> GetAll();
    Task<EmployerDTO?> GetByIdAsync(int id);
    Task<DdEmployer> Create(DdEmployer entity);
    Task Update(int id, EmployerDTO dto);
    Task Delete(int id);
}
