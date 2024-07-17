using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class ProjectAssignmentRepository : Repository<DdProjectAssignment>, IProjectAssignmentRepository
{
    public ProjectAssignmentRepository(TheCollabsysContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DdProjectAssignment>> GetAssignmentsByProjectIdAsync(int projectId)
    {
        return await _context.DD_ProjectAssignments
            .Where(ps => ps.ProjectId == projectId)
            .ToListAsync();
    }
}
