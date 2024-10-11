namespace TheCollabSys.Backend.Entity.DTOs;

public class InvitationDTO
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public Guid Token { get; set; }

    public DateTime ExpirationDate { get; set; }

    public int Status { get; set; }

    public string? Permissions { get; set; }

    public bool IsExternal { get; set; }

    public int? CompanyId { get; set; }

    public string? UserId { get; set; }
}
