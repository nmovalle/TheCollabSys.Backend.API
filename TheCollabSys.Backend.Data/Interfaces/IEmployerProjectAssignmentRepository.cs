using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Interfaces;

public interface IEmployerProjectAssignmentRepository : IRepository<DdEmployerProjectAssignment>
{
    Task<IEnumerable<DdEmployerProjectAssignment>> GetAssignmentsByEmployerIdAsync(int employerId);
}
