using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IClientRepository Clients { get; }
    IDomainRepository DomainRepository { get; }
    IUserRepository UserRepository { get; }
    Task<int> CompleteAsync();
}
