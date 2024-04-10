namespace TheCollabSys.Backend.Entity.Models;
public partial class DdEngineerEquipment
{
    public int EngineerId { get; set; }

    public int EquipmentId { get; set; }

    public int LevelId { get; set; }

    public virtual DdEngineer Engineer { get; set; } = null!;

    public virtual DdEquipment Equipment { get; set; } = null!;

    public virtual DdLevelMaster Level { get; set; } = null!;
}
