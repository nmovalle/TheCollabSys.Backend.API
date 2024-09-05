namespace TheCollabSys.Backend.Entity.DTOs;

public class InvitationModelDTO
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public Guid Token { get; set; }

    public DateTime ExpirationDate { get; set; }

    public int Status { get; set; }

    public string StatusName { get; set; }

    public string? Permissions { get; set; }

    public bool IsExternal { get; set; }

    public string Domain { get; set; } = null!;

    public int RoleId { get; set; }

    public string RoleName { get; set; }

    public bool IsBlackList { get; set; }
}
