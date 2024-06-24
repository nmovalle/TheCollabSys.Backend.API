namespace TheCollabSys.Backend.Entity.DTOs;

public class SkillDTO
{
    public int SkillId { get; set; }
    public string SkillName { get; set; } = null!;
    public int? CategoryId { get; set; }
    public string CategoryName { get; set; } = null!;
    public int? SubcategoryId { get; set; }
    public string SubcategoryName { get; set; } = null!;
}
