using Microsoft.AspNet.Identity.EntityFramework;

namespace TheCollabSys.Backend.Entity.Models;

public class ApplicationUser : IdentityUser
{
    public string? NormalizedUserName { get; set; }
    public string? NormalizedEmail { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
}
