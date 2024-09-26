namespace TheCollabSys.Backend.Entity.DTOs;

public class EngineerDTO
{
    public int EngineerId { get; set; }

    public int EmployerId { get; set; }
    public string? EmployerName { get; set; } = string.Empty!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public byte[]? Image { get; set; }

    public DateTime? DateCreated { get; set; }

    public bool IsActive { get; set; }

    public string? UserId { get; set; }

    public DateTime? DateUpdate { get; set; }

    public string? Filetype { get; set; }
    public double? Rating { get; set; }
    public int? CompanyId { get; set; }
}
