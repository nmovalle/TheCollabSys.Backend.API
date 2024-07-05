using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class MenuRepository : Repository<DdMenu>, IMenuRepository
{
    public MenuRepository(TheCollabsysContext context) : base(context)
    {
    }
}
