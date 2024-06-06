using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class ProjectSkillRepository : Repository<DdProjectSkill>, IProjectSkillRepository
{
    public ProjectSkillRepository(TheCollabsysContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DdProjectSkill>> GetSkillsByProjectIdAsync(int projectId)
    {
        return await _context.DD_ProjectSkills
            .Where(ps => ps.ProjectId == projectId)
            .ToListAsync();
    }
}
