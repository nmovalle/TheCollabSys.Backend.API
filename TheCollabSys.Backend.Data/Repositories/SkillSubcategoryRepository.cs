using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class SkillSubcategoryRepository : Repository<DdSkillSubcategory>, ISkillSubcategoryRepository
{
    public SkillSubcategoryRepository(TheCollabsysContext context) : base(context)
    {
    }
}