namespace TheCollabSys.Backend.Entity.DTOs;

public class ClientDTO : IUserOwned
{
    public int ClientId { get; set; }
    public string? ClientName { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public byte[]? Logo { get; set; }
    public string? Filetype { get; set; }
    public bool? Active { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdate { get; set; }
    public string? UserId { get; set; }
    public int? CompanyId { get; set; }
}
