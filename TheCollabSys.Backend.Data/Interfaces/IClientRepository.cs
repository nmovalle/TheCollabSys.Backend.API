using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Interfaces;

public interface IClientRepository : IRepository<DdClient>
{
    Task<IEnumerable<DdClient>> GetClientsByNameAsync(int companyId, string name);
}
