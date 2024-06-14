using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class EngineerSkillRepository : Repository<DdEngineerSkill>, IEngineerSkillRepository
{
    public EngineerSkillRepository(TheCollabsysContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DdEngineerSkill>> GetSkillsByEngineerIdAsync(int engineerId)
    {
        return await _context.DD_EngineerSkills
            .Where(ps => ps.EngineerId == engineerId)
            .ToListAsync();
    }
}
