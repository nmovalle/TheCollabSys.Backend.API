namespace TheCollabSys.Backend.Entity.DTOs;

public class UniqueIdsCategoriesDTO
{
    public List<int> CategoryIds { get; set; }
    public List<int> SubcategoryIds { get; set; }

    public UniqueIdsCategoriesDTO() {
        CategoryIds = new List<int>();
        SubcategoryIds = new List<int>();
    }
}
