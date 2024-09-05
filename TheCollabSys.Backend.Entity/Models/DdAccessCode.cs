namespace TheCollabSys.Backend.Entity.Models;

public partial class DdAccessCode
{
    public int Id { get; set; }

    public string AccessCode { get; set; }

    public string? Email { get; set; }

    public DateTime? RegAt { get; set; }

    public DateTime? ExpAt { get; set; }

    public bool? IsValid { get; set; }
}
