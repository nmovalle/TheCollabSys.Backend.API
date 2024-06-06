namespace TheCollabSys.Backend.Entity.DTOs;

public class ProjectSkillDTO
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; }

    public int SkillId { get; set; }
    public string SkillName { get; set; }

    public int LevelId { get; set; }
}
