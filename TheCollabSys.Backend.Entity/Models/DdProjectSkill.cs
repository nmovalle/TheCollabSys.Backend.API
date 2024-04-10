namespace TheCollabSys.Backend.Entity.Models;
public partial class DdProjectSkill
{
    public int ProjectId { get; set; }

    public int SkillId { get; set; }

    public int LevelId { get; set; }

    public virtual DdLevelMaster Level { get; set; } = null!;

    public virtual DdProject Project { get; set; } = null!;

    public virtual DdSkill Skill { get; set; } = null!;
}
