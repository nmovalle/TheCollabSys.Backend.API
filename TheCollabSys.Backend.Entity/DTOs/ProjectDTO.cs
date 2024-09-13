namespace TheCollabSys.Backend.Entity.DTOs;

public record ProjectDTO
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = null!;
    public int ClientId { get; set; }
    public string? ClientName { get; set; }
    public string? ProjectDescription { get; set; }
    public int? NumberPositionTobeFill { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int StatusId { get; set; }
    public string? UserId { get; set; }
    public DateTime? DateUpdate { get; set; }
    public int? CompanyId { get; set; }
}
