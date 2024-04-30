namespace TheCollabSys.Backend.Entity.Models;
public partial class DdLevelMaster
{
    public int LevelId { get; set; }

    public string LevelDescription { get; set; } = null!;

    public virtual ICollection<DdEngineerEquipment> DdEngineerEquipments { get; set; } = new List<DdEngineerEquipment>();

    public virtual ICollection<DdEngineerSkill> DdEngineerSkills { get; set; } = new List<DdEngineerSkill>();

    public virtual ICollection<DdEngineerSoftware> DdEngineerSoftwares { get; set; } = new List<DdEngineerSoftware>();

    public virtual ICollection<DdProjectEquipment> DdProjectEquipments { get; set; } = new List<DdProjectEquipment>();

    public virtual ICollection<DdProjectSkill> DdProjectSkills { get; set; } = new List<DdProjectSkill>();

    public virtual ICollection<DdProjectSoftware> DdProjectSoftwares { get; set; } = new List<DdProjectSoftware>();
}
