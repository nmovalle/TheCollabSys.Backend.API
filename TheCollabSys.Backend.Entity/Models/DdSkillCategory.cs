namespace TheCollabSys.Backend.Entity.Models;

public partial class DdSkillCategory
{
    public int CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<DdSkillSubcategory> DdSkillSubcategories { get; set; } = new List<DdSkillSubcategory>();

    public virtual ICollection<DdSkill> DdSkills { get; set; } = new List<DdSkill>();
}
