namespace TheCollabSys.Backend.Entity.DTOs;

public class EngineerSkillDetailDTO
{
    public int EngineerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public int EmployerId { get; set; }

    public string? EmployerName { get; set; } = string.Empty!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTime? DateCreated { get; set; }

    public bool IsActive { get; set; }

    public string? UserId { get; set; }

    public DateTime? DateUpdate { get; set; }

    public List<SkillLevelDTO> Skills { get; set; }
}
