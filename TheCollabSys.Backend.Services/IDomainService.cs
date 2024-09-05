using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IDomainService
{
    IAsyncEnumerable<DomainMasterDTO> GetAll();
    Task<DomainMasterDTO?> GetByIdAsync(int id);
    Task<DomainMasterDTO?> GetByDomainAsync(string domain);
    Task<DdDomainMaster> Create(DdDomainMaster entity);
}
