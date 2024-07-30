using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class EmployerProjectAssignmentRepository : Repository<DdEmployerProjectAssignment>, IEmployerProjectAssignmentRepository
{
    public EmployerProjectAssignmentRepository(TheCollabsysContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DdEmployerProjectAssignment>> GetAssignmentsByEmployerIdAsync(int employerId)
    {
        return await _context.DD_EmployerProjectAssignments
            .Where(ps => ps.EmployerId == employerId)
            .ToListAsync();
    }
}
