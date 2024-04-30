namespace TheCollabSys.Backend.Entity.Auth;

public class OAuthRequest
{
    public string? email { get; set; }
    public bool? email_verified { get; set; }
    public string? hd { get; set; }
    public string? name { get; set; }
    public string? picture { get; set; }
}
