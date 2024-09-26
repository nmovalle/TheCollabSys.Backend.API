namespace TheCollabSys.Backend.Entity.Models;
public partial class DdEngineer
{
    public int EngineerId { get; set; }

    public string EngineerName { get; set; } = string.Empty!;

    public int EmployerId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public byte[]? Image { get; set; }

    public DateTime? DateCreated { get; set; }

    public bool IsActive { get; set; }

    public string? UserId { get; set; }

    public DateTime? DateUpdate { get; set; }

    public string? Filetype { get; set; }

    public int? CompanyId { get; set; }

    public virtual DdCompany? Company { get; set; }

    public virtual ICollection<DdAssignmentHistory> DdAssignmentHistories { get; set; } = new List<DdAssignmentHistory>();

    public virtual ICollection<DdClientRating> DdClientRatings { get; set; } = new List<DdClientRating>();

    public virtual ICollection<DdEngineerActivity> DdEngineerActivities { get; set; } = new List<DdEngineerActivity>();

    public virtual ICollection<DdEngineerDailyAssignment> DdEngineerDailyAssignments { get; set; } = new List<DdEngineerDailyAssignment>();

    public virtual ICollection<DdEngineerEquipment> DdEngineerEquipments { get; set; } = new List<DdEngineerEquipment>();

    public virtual ICollection<DdEngineerRating> DdEngineerRatings { get; set; } = new List<DdEngineerRating>();

    public virtual ICollection<DdEngineerSkill> DdEngineerSkills { get; set; } = new List<DdEngineerSkill>();

    public virtual ICollection<DdEngineerSoftware> DdEngineerSoftwares { get; set; } = new List<DdEngineerSoftware>();

    public virtual ICollection<DdProjectAssignment> DdProjectAssignments { get; set; } = new List<DdProjectAssignment>();

    public virtual DdEmployer Employer { get; set; } = null!;

    public virtual AspNetUser? User { get; set; }
}
