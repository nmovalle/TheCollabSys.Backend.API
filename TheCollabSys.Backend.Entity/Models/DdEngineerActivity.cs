namespace TheCollabSys.Backend.Entity.Models;
public partial class DdEngineerActivity
{
    public int EngineerActivityId { get; set; }

    public int EngineerId { get; set; }

    public int ActivityId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? ProjectId { get; set; }

    public virtual DdActivity Activity { get; set; } = null!;

    public virtual DdEngineer Engineer { get; set; } = null!;

    public virtual DdProject? Project { get; set; }
}
