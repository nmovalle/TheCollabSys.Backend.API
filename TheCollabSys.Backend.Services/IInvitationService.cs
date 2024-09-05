using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IInvitationService
{
    IEnumerable<InvitationModelDTO> GetAll();
    Task<InvitationModelDTO?> GetByIdAsync(int id);
    Task<DdInvitation> Create(DdInvitation entity);
    Task Update(int id, InvitationDTO dto);
    Task Delete(int id);
    Task<InvitationDTO?> GetByToken(Guid token);
    Task<InvitationDTO?> GetByEmail(string email);
}
