using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Interfaces;

public interface IWireListRepository : IRepository<DdWireList>
{
    Task<DdWireList?> GetByEmail(string email);
}
