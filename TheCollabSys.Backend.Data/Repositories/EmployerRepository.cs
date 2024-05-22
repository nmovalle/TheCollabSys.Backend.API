using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class EmployerRepository : Repository<DdEmployer>, IEmployerRepository
{
    public EmployerRepository(TheCollabsysContext context) : base(context)
    {
    }
}
