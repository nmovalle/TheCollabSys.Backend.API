namespace TheCollabSys.Backend.Data.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IClientRepository Clients { get; }
    IDomainRepository DomainRepository { get; }
    IUserRepository UserRepository { get; }
    IUserRoleRepository UserRoleRepository { get; }
    ITokenRepository TokenRepository { get; }
    IRoleRepository RoleRepository { get; }
    IProposalRoleRepository ProposalRoleRepository { get; }
    IEmployerRepository EmployerRepository { get; }
    ISkillRepository SkillRepository { get; }
    IProjectRepository ProjectRepository { get; }
    IProjectSkillRepository ProjectSkillRepository { get; }
    IEngineerRepository EngineerRepository { get; }
    IEngineerSkillRepository EngineerSkillRepository { get; }
    ISkillCategoryRepository SkillCategoryRepository { get; }
    ISkillSubcategoryRepository SkillSubcategoryRepository { get; }
    IMenuRepository MenuRepository { get; }
    ISubMenuRepository SubMenuRepository { get; }
    IMenuRolesRepository MenuRolesRepository { get; }
    IProjectAssignmentRepository ProjectAssignmentRepository { get; }
    IEmployerProjectAssignmentRepository EmployerProjectAssignmentRepository { get; }
    IAccessCodeRepsitory AccessCodeRepsitory { get; }
    ICompanyRepository CompanyRepository { get; }
    IUserCompanyRepository UserCompanyRepository { get; }
    IInvitationRepository InvitationRepository { get; }
    IWireListRepository WireListRepository { get; }
    IStatusRepository StatusRepository { get; }

    Task<int> CompleteAsync();
}
