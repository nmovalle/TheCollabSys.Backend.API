namespace TheCollabSys.Backend.Entity.Models;
public partial class DdProposalRole
{
    public int ProposalId { get; set; }

    public string? ProposalRoles { get; set; }

    public string? RoleId { get; set; }

    public string? ProposalName { get; set; }

    public string? Description { get; set; }

    public virtual AspNetRole? Role { get; set; }
}
