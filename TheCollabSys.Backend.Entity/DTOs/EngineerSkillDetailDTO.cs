namespace TheCollabSys.Backend.Entity.DTOs;

public class EngineerSkillDetailDTO
{
    public int EngineerId { get; set; }
    public string EngineerName { get; set; }

    public List<SkillLevelDTO> Skills { get; set; }
}
