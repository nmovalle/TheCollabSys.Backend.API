namespace TheCollabSys.Backend.Entity.Models;
public partial class DdProfile
{
    public int ProfileId { get; set; }

    public string? ProfileName { get; set; }

    public virtual ICollection<DdProfileConfigAction> DdProfileConfigActions { get; set; } = new List<DdProfileConfigAction>();

    public virtual ICollection<DdProfileConfigView> DdProfileConfigViews { get; set; } = new List<DdProfileConfigView>();

    public virtual ICollection<DdUsersProfile> DdUsersProfiles { get; set; } = new List<DdUsersProfile>();
}
