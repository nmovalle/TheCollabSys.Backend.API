namespace TheCollabSys.Backend.Entity.Models;
public partial class DdEngineerSkill
{
    public int EngineerId { get; set; }

    public int SkillId { get; set; }

    public int LevelId { get; set; }

    public virtual DdEngineer Engineer { get; set; } = null!;

    public virtual DdLevelMaster Level { get; set; } = null!;

    public virtual DdSkill Skill { get; set; } = null!;
}
