using TheCollabSys.Backend.Entity.DTOs;

namespace TheCollabSys.Backend.Services;

public interface IEmployerProjectAssignmentService
{
    IAsyncEnumerable<EmployerProjectAssignmentDetailDTO> GetAll();
    Task<EmployerProjectAssignmentDetailDTO?> GetByIdAsync(int id);
    Task Create(EmployerProjectAssignmentDetailDTO entity);
    Task Update(int id, EmployerProjectAssignmentDetailDTO dto);
    Task Delete(int id);
}
