namespace TheCollabSys.Backend.Entity.Models;

public partial class DdMenuRole
{
    public int MenuRoleId { get; set; }

    public string RoleId { get; set; } = null!;

    public int SubMenuId { get; set; }

    public bool? View { get; set; }

    public bool? Add { get; set; }

    public bool? Edit { get; set; }

    public bool? Delete { get; set; }

    public bool? Export { get; set; }

    public bool? Import { get; set; }

    public virtual AspNetRole Role { get; set; } = null!;

    public virtual DdSubMenu SubMenu { get; set; } = null!;
}
