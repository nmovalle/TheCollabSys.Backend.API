namespace TheCollabSys.Backend.Entity.DTOs;

public class MenuRoleDetailDTO
{
    public string label { get; set; } = null;
    public string icon { get; set; } = null;
    public int menuId { get; set; }
    public string roleId { get; set; }
    public List<SubMenuRoleDTO> items { get; set; }
    public MenuRoleDetailDTO()
    {
        items = new List<SubMenuRoleDTO>();
    }
}

public class SubMenuRoleDTO
{
    public string roleId { get; set; }
    public string roleName { get; set; }
    public string label { get; set; } = null;
    public string icon { get; set; } = null;
    public string routerLink { get; set; } = null;
    public bool? view { get; set; } = null;
    public bool? add { get; set; } = null;
    public bool? edit { get; set; } = null;
    public bool? delete { get; set; } = null;
    public bool? export { get; set; } = null;
    public bool? import { get; set; } = null;
}