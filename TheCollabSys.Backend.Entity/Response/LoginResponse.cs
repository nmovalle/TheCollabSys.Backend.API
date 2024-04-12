using TheCollabSys.Backend.Entity.DTOs;

namespace TheCollabSys.Backend.Entity.Response;

public class LoginResponse
{
    public UserRoleDTO UserRole { get; set; }
    public AuthTokenResponse AuthToken { get; set; }
}
