﻿using TheCollabSys.Backend.API.Controllers;
using TheCollabSys.Backend.API.Filters;
using TheCollabSys.Backend.API.Middlewares;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Configuration;

public class InfrastructureServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IDomainService, DomainService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRoleService, UserRoleService>();

        services.AddScoped(typeof(IMapperService<,>), typeof(MapperService<,>));

        services.AddAutoMapper(typeof(Program));
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddTransient<ExceptionHandlingMiddleware>();
        services.AddScoped<GlobalExceptionFilter>();
        services.AddScoped<ModelStateFilter>();
    }
}
