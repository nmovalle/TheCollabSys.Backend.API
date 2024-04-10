using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Interfaces;

public interface IDomainRepository : IRepository<DdDomainMaster>
{
    Task<DdDomainMaster?> GetDomainMasterByDomain(string domain);
}
