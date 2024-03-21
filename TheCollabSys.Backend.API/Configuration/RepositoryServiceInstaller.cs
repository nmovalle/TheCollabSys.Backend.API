using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Data.Repositories;

namespace TheCollabSys.Backend.API.Configuration;

public class RepositoryServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TheCollabsysContext>(options => options.UseSqlServer(configuration.GetConnectionString(nameof(TheCollabsysContext))));
        services.AddScoped<DbContext, TheCollabsysContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
