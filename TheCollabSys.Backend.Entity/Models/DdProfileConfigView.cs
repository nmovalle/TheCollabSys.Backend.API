namespace TheCollabSys.Backend.Entity.Models;
public partial class DdProfileConfigView
{
    public int ConfigId { get; set; }

    public int? ProfileId { get; set; }

    public int? ViewId { get; set; }

    public virtual DdProfile? Profile { get; set; }

    public virtual DdView? View { get; set; }
}
