namespace TheCollabSys.Backend.Entity.DTOs;

public class CompanyDTO
{
    public int CompanyId { get; set; }

    public int? DomainmasterId { get; set; }

    public string? FullName { get; set; }

    public string? Address { get; set; }

    public int? Zipcode { get; set; }

    public string? Phone { get; set; }

    public byte[]? Logo { get; set; }

    public string? FileType { get; set; }

    public bool? Active { get; set; }
}
