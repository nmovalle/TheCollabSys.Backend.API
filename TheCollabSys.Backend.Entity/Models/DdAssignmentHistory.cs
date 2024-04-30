namespace TheCollabSys.Backend.Entity.Models;
public partial class DdAssignmentHistory
{
    public int HistoryId { get; set; }

    public int ProjectId { get; set; }

    public int EngineerId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime AssignedDate { get; set; }

    public virtual DdEngineer Engineer { get; set; } = null!;

    public virtual DdProject Project { get; set; } = null!;
}
