namespace TheCollabSys.Backend.Entity.DTOs;

public class SubMenuDTO
{
    public int SubMenuId { get; set; }

    public int MenuId { get; set; }
    public string? MenuName { get; set; }

    public string? SubMenuName { get; set; }

    public string? Description { get; set; }

    public string? Icon { get; set; }

    public string? RouterLink { get; set; }
}
