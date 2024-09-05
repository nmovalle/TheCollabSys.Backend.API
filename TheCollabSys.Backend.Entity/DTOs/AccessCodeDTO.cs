namespace TheCollabSys.Backend.Entity.DTOs;

public class AccessCodeDTO
{
    public int Id { get; set; }

    public string AccessCode { get; set; }

    public string? Email { get; set; }

    public DateTime? RegAt { get; set; } = DateTime.UtcNow;

    public DateTime? ExpAt { get; set; }

    public bool? IsValid { get; set; }
}
