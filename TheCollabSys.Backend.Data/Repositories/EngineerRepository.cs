using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class EngineerRepository : Repository<DdEngineer>, IEngineerRepository
{
    public EngineerRepository(TheCollabsysContext context) : base(context)
    {
    }
}