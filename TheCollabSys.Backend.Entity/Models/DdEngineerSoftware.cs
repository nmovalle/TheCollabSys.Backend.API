namespace TheCollabSys.Backend.Entity.Models;
public partial class DdEngineerSoftware
{
    public int EngineerId { get; set; }

    public int SoftwareId { get; set; }

    public int LevelId { get; set; }

    public virtual DdEngineer Engineer { get; set; } = null!;

    public virtual DdLevelMaster Level { get; set; } = null!;

    public virtual DdSoftware Software { get; set; } = null!;
}
