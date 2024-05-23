using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class SkillRepository : Repository<DdSkill>, ISkillRepository
{
    public SkillRepository(TheCollabsysContext context) : base(context)
    {
    }
}
