namespace TheCollabSys.Backend.Entity.Models;
public partial class DdView
{
    public int ViewId { get; set; }

    public string? ViewName { get; set; }

    public virtual ICollection<DdProfileConfigAction> DdProfileConfigActions { get; set; } = new List<DdProfileConfigAction>();

    public virtual ICollection<DdProfileConfigView> DdProfileConfigViews { get; set; } = new List<DdProfileConfigView>();
}
