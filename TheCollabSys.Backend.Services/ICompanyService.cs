using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface ICompanyService
{
    IAsyncEnumerable<CompanyDTO> GetAll(int companyId);
    Task<CompanyDTO?> GetByIdAsync(int id);
    Task<CompanyDTO?> GetByIdDomainAsync(string domain);
    Task<DdCompany> Create(DdCompany entity);
    Task Update(int id, CompanyDTO dto);
    Task Delete(int id);
}
