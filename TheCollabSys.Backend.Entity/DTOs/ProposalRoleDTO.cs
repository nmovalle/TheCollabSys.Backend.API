namespace TheCollabSys.Backend.Entity.DTOs;

public class ProposalRoleDTO
{
    public int ProposalId { get; set; }

    public string? ProposalRoles { get; set; } = null;

    public string? RoleId { get; set; } = null;

    public string? ProposalName { get; set; } = null;

    public string? Description { get; set; } = null;
}
