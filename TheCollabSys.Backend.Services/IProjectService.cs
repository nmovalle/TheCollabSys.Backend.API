
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IProjectService
{
    IAsyncEnumerable<ProjectDTO> GetAll(int companyId);
    Task<ProjectDTO?> GetByIdAsync(int companyId, int id);
    Task<DdProject> Create(DdProject entity);
    Task Update(int id, ProjectDTO dto);
    Task Delete(int id);
    Task<dynamic> GetKPIs();
}
