using TheCollabSys.Backend.API.Filters;
using TheCollabSys.Backend.API.Middlewares;
using TheCollabSys.Backend.API.Token;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Configuration;

public class InfrastructureServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddScoped<IDomainService, DomainService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IProposalRoleService, ProposalRoleService>();

        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IEmployerService, EmployerService>();
        services.AddScoped<ISkillService, SkillService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IProjectSkillService, ProjectSkillService>();
        services.AddScoped<IEngineerService, EngineerService>();
        services.AddScoped<IEngineerSkillService, EngineerSkillService>();


        services.AddScoped(typeof(IMapperService<,>), typeof(MapperService<,>));

        services.AddAutoMapper(typeof(Program));
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddTransient<ExceptionHandlingMiddleware>();
        services.AddScoped<GlobalExceptionFilter>();
        services.AddScoped<ModelStateFilter>();

        services.AddScoped<UserIdFilter>();
    }
}
