namespace TheCollabSys.Backend.Entity.DTOs;

public class AspNetUserTokensDTO
{
    public string UserId { get; set; } = null!;

    public string LoginProvider { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Value { get; set; }
}
