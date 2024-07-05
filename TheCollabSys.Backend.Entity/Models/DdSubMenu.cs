namespace TheCollabSys.Backend.Entity.Models;

public partial class DdSubMenu
{
    public int SubMenuId { get; set; }

    public int MenuId { get; set; }

    public string? SubMenuName { get; set; }

    public string? Description { get; set; }

    public string? Icon { get; set; }

    public string? RouterLink { get; set; }

    public virtual ICollection<DdMenuRole> DdMenuRoles { get; set; } = new List<DdMenuRole>();

    public virtual DdMenu Menu { get; set; } = null!;
}

