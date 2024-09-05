using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IAccessCodeService
{
    IAsyncEnumerable<AccessCodeDTO> GetAll();
    Task<AccessCodeDTO?> GetByIdAsync(int id);
    Task<DdAccessCode> Create(DdAccessCode entity);
    Task Update(int id, AccessCodeDTO dto);
    Task Delete(int id);
    Task<AccessCodeDTO?> GetByEmail(string email);
    Task<AccessCodeDTO?> GetByAccessCodeEmail(string accessCode, string email);
}
