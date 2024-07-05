using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class MenuRolesRepository : Repository<DdMenuRole>, IMenuRolesRepository
{
    public MenuRolesRepository(TheCollabsysContext context) : base(context)
    {
    }
}
