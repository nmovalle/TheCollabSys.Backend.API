namespace TheCollabSys.Backend.Entity.Request;

public class InvitationRequest
{
    public string Email { get; set; } = null!;
    public string Domain { get; set; } = null!;
    public bool IsExternal { get; set; }
    public int RoleId { get; set; }
    public bool IsBlackList { get; set; }
}
