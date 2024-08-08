using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IUserCompanyService
{
    IAsyncEnumerable<UserCompanyDTO> GetAll();
    Task<UserCompanyDTO?> GetByIdAsync(int id);
    Task<DdUserCompany> Create(DdUserCompany entity);
    Task Update(int id, UserCompanyDTO dto);
    Task Delete(int id);
}
