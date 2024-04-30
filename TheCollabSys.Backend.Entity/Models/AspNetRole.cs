﻿namespace TheCollabSys.Backend.Entity.Models;
public partial class AspNetRole
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? NormalizedName { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; } = new List<AspNetRoleClaim>();

    public virtual ICollection<DdProposalRole> DdProposalRoles { get; set; } = new List<DdProposalRole>();

    public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();

    public virtual ICollection<AspNetUserRole> UserRoles { get; set; } = new List<AspNetUserRole>();
}
