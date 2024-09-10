namespace TheCollabSys.Backend.Entity.DTOs;

public class WireListDTO
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Domain { get; set; } = null!;

    public bool IsExternal { get; set; }

    public int RoleId { get; set; }

    public bool IsBlackList { get; set; }

    public bool PasswordConfirmed { get; set; }
}
