namespace TheCollabSys.Backend.Entity.DTOs;

public record EmployerDTO
{
    public int EmployerId { get; init; }
    public string EmployerName { get; init; }
    public string? Address { get; init; }
    public string? Phone { get; init; }
    public byte[]? Image { get; init; }
    public string? Filetype { get; set; }
    public DateTime? DateCreated { get; init; }
    public DateTime? DateUpdate { get; set; }
    public bool? Active { get; set; }
    public string? UserId { get; init; }
}
