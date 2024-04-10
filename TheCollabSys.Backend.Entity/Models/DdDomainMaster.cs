namespace TheCollabSys.Backend.Entity.Models;
public partial class DdDomainMaster
{
    public int Id { get; set; }

    public string? Domain { get; set; }

    public string? FullName { get; set; }

    public bool? Active { get; set; }

    public DateTime? DateCreated { get; set; }
}
