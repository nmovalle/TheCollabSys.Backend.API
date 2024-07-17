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
    public IProjectSkillRepository ProjectSkillRepository { get; private set; }
    public IEngineerRepository EngineerRepository { get; private set; }
    public IEngineerSkillRepository EngineerSkillRepository { get; private set; }
    public ISkillCategoryRepository SkillCategoryRepository { get; private set; }
    public ISkillSubcategoryRepository SkillSubcategoryRepository { get; private set; }
    public IMenuRepository MenuRepository { get; private set; }
    public ISubMenuRepository SubMenuRepository { get; private set; }
    public IMenuRolesRepository MenuRolesRepository { get; private set; }
    public IProjectAssignmentRepository ProjectAssignmentRepository { get; private set; }


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
        ProjectSkillRepository = new ProjectSkillRepository(_context);
        EngineerRepository = new EngineerRepository(_context);
        EngineerSkillRepository = new EngineerSkillRepository(_context);
        SkillCategoryRepository = new SkillCategoryRepository(_context);
        SkillSubcategoryRepository = new SkillSubcategoryRepository(_context);
        MenuRepository = new MenuRepository(_context);
        SubMenuRepository = new SubMenuRepository(_context);
        MenuRolesRepository = new MenuRolesRepository(_context);
        ProjectAssignmentRepository = new ProjectAssignmentRepository(_context);
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
