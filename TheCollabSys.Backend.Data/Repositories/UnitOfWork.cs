using TheCollabSys.Backend.Data.Interfaces;

namespace TheCollabSys.Backend.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly TheCollabsysContext _context;

    public IClientRepository Clients { get; private set; }
    public IDomainRepository DomainRepository { get; private set; }
    public IUserRepository UserRepository { get; private set; }
    public IUserRoleRepository UserRoleRepository { get; private set; }
    public ITokenRepository TokenRepository { get; private set; }
    public IRoleRepository RoleRepository { get; private set; }
    public IProposalRoleRepository ProposalRoleRepository { get; private set; }
    public IEmployerRepository EmployerRepository { get; private set; }
    public ISkillRepository SkillRepository { get; private set; }
    public IProjectRepository ProjectRepository { get; private set; }


    public UnitOfWork(TheCollabsysContext context)
    {
        _context = context; 

        Clients = new ClientRepository(_context);
        DomainRepository = new DomainRepository(_context);
        UserRepository = new UserRepository(_context);
        UserRoleRepository = new UserRoleRepository(_context);
        TokenRepository = new TokenRepository(_context);
        RoleRepository = new RoleRepository(_context);
        ProposalRoleRepository = new ProposalRoleRepository(_context);
        EmployerRepository = new EmployerRepository(_context);
        SkillRepository = new SkillRepository(_context);
        ProjectRepository = new ProjectRepository(_context);
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
