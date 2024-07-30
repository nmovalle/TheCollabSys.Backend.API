namespace TheCollabSys.Backend.Entity.Models;
public partial class DdProject
{
    public int ProjectId { get; set; }

    public string ProjectName { get; set; } = null!;

    public int ClientId { get; set; }

    public string? ProjectDescription { get; set; }

    public int? NumberPositionTobeFill { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int StatusId { get; set; }

    public string? UserId { get; set; }

    public DateTime? DateUpdate { get; set; }

    public virtual ICollection<DdAssignmentHistory> DdAssignmentHistories { get; set; } = new List<DdAssignmentHistory>();

    public virtual ICollection<DdEmployerProjectAssignment> DdEmployerProjectAssignments { get; set; } = new List<DdEmployerProjectAssignment>();

    public virtual ICollection<DdEngineerActivity> DdEngineerActivities { get; set; } = new List<DdEngineerActivity>();

    public virtual ICollection<DdEngineerDailyAssignment> DdEngineerDailyAssignments { get; set; } = new List<DdEngineerDailyAssignment>();

    public virtual ICollection<DdProjectAssignment> DdProjectAssignments { get; set; } = new List<DdProjectAssignment>();

    public virtual ICollection<DdProjectEquipment> DdProjectEquipments { get; set; } = new List<DdProjectEquipment>();

    public virtual ICollection<DdProjectSkill> DdProjectSkills { get; set; } = new List<DdProjectSkill>();

    public virtual ICollection<DdProjectSoftware> DdProjectSoftwares { get; set; } = new List<DdProjectSoftware>();

    public virtual DdProjectStatus Status { get; set; } = null!;
}
