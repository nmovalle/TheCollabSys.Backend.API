using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IDomainService
{
    Task<DdDomainMaster> AddDomainMaster(DdDomainMaster domainMaster);
    Task<IEnumerable<DdDomainMaster>> GetAllDomainMastersAsync();
    Task<DdDomainMaster?> GetDomainMasterById(int id);
    Task<DdDomainMaster?> GetDomainMasterByDomain(string domain);
}
