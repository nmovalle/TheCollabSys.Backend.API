using TheCollabSys.Backend.Entity.DTOs;

namespace TheCollabSys.Backend.Services;

public interface IEmployerProjectAssignmentService
{
    IAsyncEnumerable<EmployerProjectAssignmentDetailDTO> GetAll(int companyId);
    Task<EmployerProjectAssignmentDetailDTO?> GetByIdAsync(int companyId, int id);
    Task Create(EmployerProjectAssignmentDetailDTO entity);
    Task Update(int id, EmployerProjectAssignmentDetailDTO dto);
    Task Delete(int id);
}
