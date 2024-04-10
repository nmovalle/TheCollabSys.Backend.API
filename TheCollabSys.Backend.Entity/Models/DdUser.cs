namespace TheCollabSys.Backend.Entity.Models;
public partial class DdUser
{
    public int UserId { get; set; }

    public string? Username { get; set; }

    public byte[]? PasswordHash { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<DdUsersProfile> DdUsersProfiles { get; set; } = new List<DdUsersProfile>();
}
