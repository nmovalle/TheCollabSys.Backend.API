namespace TheCollabSys.Backend.Entity.Models;
public partial class DdClientRating
{
    public int RatingId { get; set; }

    public int EngineerId { get; set; }

    public int ClientId { get; set; }

    public int Rating { get; set; }

    public string? Comments { get; set; }

    public DateTime DateCreated { get; set; }

    public virtual DdClient Client { get; set; } = null!;

    public virtual DdEngineer Engineer { get; set; } = null!;
}
