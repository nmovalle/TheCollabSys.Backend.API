using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class SubMenuRepository : Repository<DdSubMenu>, ISubMenuRepository
{
    public SubMenuRepository(TheCollabsysContext context) : base(context)
    {
    }
}
