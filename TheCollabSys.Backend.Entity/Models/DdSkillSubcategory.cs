namespace TheCollabSys.Backend.Entity.Models;

public partial class DdSkillSubcategory
{
    public int SubcategoryId { get; set; }

    public string SubcategoryName { get; set; } = null!;

    public int CategoryId { get; set; }

    public virtual DdSkillCategory Category { get; set; } = null!;

    public virtual ICollection<DdSkill> DdSkills { get; set; } = new List<DdSkill>();
}
