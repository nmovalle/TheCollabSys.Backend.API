using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Interfaces;

public interface IClientRepository : IRepository<DD_Clients>
{
    Task<IEnumerable<DD_Clients>> GetClientsByNameAsync(string name);
}
