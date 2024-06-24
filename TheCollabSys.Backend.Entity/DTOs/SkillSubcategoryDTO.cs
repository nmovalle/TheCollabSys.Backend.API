namespace TheCollabSys.Backend.Entity.DTOs;

public class SkillSubcategoryDTO
{
    public int SubcategoryId { get; set; }
    public string SubcategoryName { get; set; } = null!;
    public int CategoryId { get; set; }
    public string? CategoryName { get; set; } = null!;

}
