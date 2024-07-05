namespace TheCollabSys.Backend.Entity.Models;

public partial class DdMenu
{
    public int MenuId { get; set; }

    public string? MenuName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<DdSubMenu> DdSubMenus { get; set; } = new List<DdSubMenu>();
}
