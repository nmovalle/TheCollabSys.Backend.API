namespace TheCollabSys.Backend.Entity.Models;
public partial class DdClient
{
    public int ClientId { get; set; }

    public string ClientName { get; set; } = null!;

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public byte[]? Logo { get; set; }

    public DateTime? DateCreated { get; set; }

    public string? Domain { get; set; }

    public bool? Active { get; set; }

    public string? UserId { get; set; }

    public virtual ICollection<DdClientRating> DdClientRatings { get; set; } = new List<DdClientRating>();

    public virtual ICollection<DdEngineerRating> DdEngineerRatings { get; set; } = new List<DdEngineerRating>();

    public virtual ICollection<DdProject> DdProjects { get; set; } = new List<DdProject>();

    public virtual AspNetUser? User { get; set; }
}
