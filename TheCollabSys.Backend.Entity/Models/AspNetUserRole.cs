namespace TheCollabSys.Backend.Entity.Models;

public class AspNetUserRole
{
    public string UserId { get; set; }
    public string RoleId { get; set; }

    public virtual AspNetUser User { get; set; }
    public virtual AspNetRole Role { get; set; }
}
