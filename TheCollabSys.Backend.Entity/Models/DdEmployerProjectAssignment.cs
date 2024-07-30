namespace TheCollabSys.Backend.Entity.Models;

public partial class DdEmployerProjectAssignment
{
    public int AssignmentId { get; set; }

    public int EmployerId { get; set; }

    public int ProjectId { get; set; }

    public DateTime? DateAssigned { get; set; }

    public virtual DdEmployer Employer { get; set; } = null!;

    public virtual DdProject Project { get; set; } = null!;
}

