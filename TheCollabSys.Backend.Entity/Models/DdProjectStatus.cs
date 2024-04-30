namespace TheCollabSys.Backend.Entity.Models;
public partial class DdProjectStatus
{
    public int StatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public virtual ICollection<DdProject> DdProjects { get; set; } = new List<DdProject>();
}
