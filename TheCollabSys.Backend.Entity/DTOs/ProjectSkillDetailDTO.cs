namespace TheCollabSys.Backend.Entity.DTOs;

public class ProjectSkillDetailDTO
{
    public int ProjectId { get; set; }
    public string ProjectName { get; set; }

    public List<SkillLevelDTO> Skills { get; set; }
}

public class SkillLevelDTO
{
    public int SkillId { get; set; }
    public string SkillName { get; set; }
    public int LevelId { get; set; }
}