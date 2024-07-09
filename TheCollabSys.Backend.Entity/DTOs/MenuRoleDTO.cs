namespace TheCollabSys.Backend.Entity.DTOs;

public class MenuRoleDTO
{
    public int MenuRoleId { get; set; }

    public string RoleId { get; set; } = null!;
    public string RoleName { get; set; } = null!;

    public int SubMenuId { get; set; }

    public string SubMenuName { get; set; }

    public bool? View { get; set; }

    public bool? Add { get; set; }

    public bool? Edit { get; set; }

    public bool? Delete { get; set; }

    public bool? Export { get; set; }

    public bool? Import { get; set; }
}
