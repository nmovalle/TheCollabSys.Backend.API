using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Interfaces;

public interface IAccessCodeRepsitory : IRepository<DdAccessCode>
{
    Task<DdAccessCode?> GetByEmail(string email);
    Task<DdAccessCode?> GetByAccessCodeEmail(string accessCode, string email);
}
