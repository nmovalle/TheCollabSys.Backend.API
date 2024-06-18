using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class SkillCategoryRepository : Repository<DdSkillCategory>, ISkillCategoryRepository
{
    public SkillCategoryRepository(TheCollabsysContext context) : base(context)
    {
    }
}
