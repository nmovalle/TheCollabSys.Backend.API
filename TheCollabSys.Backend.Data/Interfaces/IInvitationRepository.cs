using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Interfaces;

public interface IInvitationRepository : IRepository<DdInvitation>
{
    Task<DdInvitation?> GetByToken(Guid token);
    Task<DdInvitation?> GetByEmail(string email);
}
