using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly TheCollabsysContext _context;
    public IClientRepository Clients { get; private set; }
    public IDomainRepository DomainRepository { get; private set; }
    public IUserRepository UserRepository { get; private set; }
    public IUserRoleRepository UserRoleRepository { get; private set; }

    public UnitOfWork(TheCollabsysContext context)
    {
        _context = context;
        Clients = new ClientRepository(_context);
        DomainRepository = new DomainRepository(_context);
        UserRepository = new UserRepository(_context);
        UserRoleRepository = new UserRoleRepository(_context);
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
