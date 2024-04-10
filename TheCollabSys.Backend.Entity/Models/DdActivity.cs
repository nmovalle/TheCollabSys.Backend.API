namespace TheCollabSys.Backend.Entity.Models;
public partial class DdActivity
{
    public int ActivityId { get; set; }

    public string ActivityName { get; set; } = null!;

    public virtual ICollection<DdEngineerActivity> DdEngineerActivities { get; set; } = new List<DdEngineerActivity>();

    public virtual ICollection<DdEngineerDailyAssignment> DdEngineerDailyAssignments { get; set; } = new List<DdEngineerDailyAssignment>();
}
