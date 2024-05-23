﻿namespace TheCollabSys.Backend.Data.Interfaces;

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

    Task<int> CompleteAsync();
}
