namespace TheCollabSys.Backend.Entity.Models;
public partial class DdSkill
{
    public int SkillId { get; set; }

    public string SkillName { get; set; } = null!;

    public int? CategoryId { get; set; }

    public int? SubcategoryId { get; set; }

    public virtual DdSkillCategory? Category { get; set; }

    public virtual ICollection<DdEngineerSkill> DdEngineerSkills { get; set; } = new List<DdEngineerSkill>();

    public virtual ICollection<DdProjectSkill> DdProjectSkills { get; set; } = new List<DdProjectSkill>();

    public virtual DdSkillSubcategory? Subcategory { get; set; }
}
