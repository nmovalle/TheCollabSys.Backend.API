namespace TheCollabSys.Backend.Entity.Models;
public partial class DdProjectSoftware
{
    public int ProjectId { get; set; }

    public int SoftwareId { get; set; }

    public int LevelId { get; set; }

    public virtual DdLevelMaster Level { get; set; } = null!;

    public virtual DdProject Project { get; set; } = null!;

    public virtual DdSoftware Software { get; set; } = null!;
}
