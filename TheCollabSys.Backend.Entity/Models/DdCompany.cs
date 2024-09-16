namespace TheCollabSys.Backend.Entity.Models;

public partial class DdCompany
{
    public int CompanyId { get; set; }

    public int? DomainmasterId { get; set; }

    public string? FullName { get; set; }

    public string? Address { get; set; }

    public int? Zipcode { get; set; }

    public string? Phone { get; set; }

    public byte[]? Logo { get; set; }

    public string? FileType { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<DdClient> DdClients { get; set; } = new List<DdClient>();

    public virtual ICollection<DdEmployer> DdEmployers { get; set; } = new List<DdEmployer>();

    public virtual ICollection<DdEngineer> DdEngineers { get; set; } = new List<DdEngineer>();

    public virtual ICollection<DdProject> DdProjects { get; set; } = new List<DdProject>();
}
