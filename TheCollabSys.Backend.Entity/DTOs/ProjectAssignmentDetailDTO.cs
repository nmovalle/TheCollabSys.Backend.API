namespace TheCollabSys.Backend.Entity.DTOs;

public class ProjectAssignmentDetailDTO
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; }
    public List<Assignments> Assignments { get; set; }
}

public class Assignments
{
    public int EngineerId { get; set; }
    public string EngineerName { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public byte[]? Image { get; set; }
    public int Rating { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? UpdatedDate { get; set; }
}