namespace TheCollabSys.Backend.Entity.Models;
public partial class DdUsersProfile
{
    public int UserProfileId { get; set; }

    public int? UserId { get; set; }

    public int? ProfileId { get; set; }

    public virtual DdProfile? Profile { get; set; }

    public virtual DdUser? User { get; set; }
}
