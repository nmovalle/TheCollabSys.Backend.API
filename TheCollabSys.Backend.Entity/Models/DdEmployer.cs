namespace TheCollabSys.Backend.Entity.Models;
public partial class DdEmployer
{
    public int EmployerId { get; set; }

    public string EmployerName { get; set; } = null!;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public DateTime? DateCreated { get; set; }

    public byte[]? Image { get; set; }

    public string? UserId { get; set; }

    public virtual ICollection<DdEngineer> DdEngineers { get; set; } = new List<DdEngineer>();

    public virtual AspNetUser? User { get; set; }
}
