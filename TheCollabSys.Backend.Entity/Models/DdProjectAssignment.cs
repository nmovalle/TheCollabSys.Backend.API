namespace TheCollabSys.Backend.Entity.Models;
public partial class DdProjectAssignment
{
    public int AssignmentId { get; set; }

    public int ProjectId { get; set; }

    public int EngineerId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? DateCreated { get; set; }

    public virtual DdEngineer Engineer { get; set; } = null!;

    public virtual DdProject Project { get; set; } = null!;
}
