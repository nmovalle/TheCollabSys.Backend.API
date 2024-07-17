using TheCollabSys.Backend.Entity.DTOs;

namespace TheCollabSys.Backend.Services;

public interface IProjectAssignmentService
{
    IAsyncEnumerable<ProjectAssignmentDetailDTO> GetAll();
    Task<ProjectAssignmentDetailDTO?> GetByIdAsync(int id);
    Task Create(ProjectAssignmentDetailDTO entity);
    Task Update(int id, ProjectAssignmentDetailDTO dto);
    Task Delete(int id);
}
