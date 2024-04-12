using Microsoft.AspNetCore.Authentication.JwtBearer;
using TheCollabSys.Backend.API.OptionSetup;

namespace TheCollabSys.Backend.API.Configuration;

public class AutenticationServiceIstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
    }
}
