using Microsoft.Extensions.Options;

namespace TheCollabSys.Backend.API.OptionSetup;

public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
{
    private const string SectionName = "Jwt";
    private readonly IConfiguration _configuration;
    public JwtOptionsSetup(IConfiguration configuration)
    {
        this._configuration = configuration;
    }

    public void Configure(JwtOptions options)
    {
        this._configuration.GetSection(SectionName).Bind(options);
    }
}
