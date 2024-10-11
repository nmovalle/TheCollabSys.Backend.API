using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IInvitationService
{
    IEnumerable<InvitationModelDTO> GetAll(int companyId);
    Task<InvitationModelDTO?> GetByIdAsync(int id, int companyId);
    Task<DdInvitation> Create(DdInvitation entity);
    Task Update(int id, InvitationDTO dto);
    Task Delete(int id);
    Task<InvitationDTO?> GetByToken(Guid token);
    Task<InvitationDTO?> GetByEmail(string email);
}
