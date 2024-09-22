using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class StatusRepository : Repository<DdStatus>, IStatusRepository
{
    public StatusRepository(TheCollabsysContext context) : base(context)
    {
    }
}