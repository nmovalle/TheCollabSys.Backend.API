using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class ProjectRepository : Repository<DdProject>, IProjectRepository
{
    public ProjectRepository(TheCollabsysContext context) : base(context)
    {
    }
}
