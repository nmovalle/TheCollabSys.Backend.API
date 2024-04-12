namespace TheCollabSys.Backend.Entity.Response;

public class AuthTokenResponse
{
    public string AccessToken { get; set; }
    public DateTime AccessTokenExpiration { get; set; }
    public string RefreshToken { get; set; }
}
