namespace TheCollabSys.Backend.Entity.Models;
public partial class DdProjectEquipment
{
    public int ProjectId { get; set; }

    public int EquipmentId { get; set; }

    public int LevelId { get; set; }

    public virtual DdEquipment Equipment { get; set; } = null!;

    public virtual DdLevelMaster Level { get; set; } = null!;

    public virtual DdProject Project { get; set; } = null!;
}
