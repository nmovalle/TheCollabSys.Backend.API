namespace TheCollabSys.Backend.Entity.Models;
public partial class DdProfileConfigAction
{
    public int ActionId { get; set; }

    public int? ProfileId { get; set; }

    public int? FViewId { get; set; }

    public bool? FRead { get; set; }

    public bool? FAdd { get; set; }

    public bool? FUpdate { get; set; }

    public bool? FRemove { get; set; }

    public virtual DdView? FView { get; set; }

    public virtual DdProfile? Profile { get; set; }
}
