namespace TheCollabSys.Backend.API.OptionSetup;

public class JwtOptions
{
    public string? Issuer { get; init; }
    public string? Audience { get; init; }
    public string? SecretKey { get; init; }
    public int ExpireMinutes { get; set; }
}
