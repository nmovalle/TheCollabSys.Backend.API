namespace TheCollabSys.Backend.Entity.Models;
public partial class DdSoftware
{
    public int SoftwareId { get; set; }

    public string SoftwareName { get; set; } = null!;

    public virtual ICollection<DdEngineerSoftware> DdEngineerSoftwares { get; set; } = new List<DdEngineerSoftware>();

    public virtual ICollection<DdProjectSoftware> DdProjectSoftwares { get; set; } = new List<DdProjectSoftware>();
}
