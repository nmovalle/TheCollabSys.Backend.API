using System.Xml.Linq;

namespace TheCollabSys.Backend.Entity.DTOs;

public class SkillCategoriesDetailDTO
{
    public string key { get; set; } = null;
    public string label { get; set; } = null;
    public string data { get; set; } = null;
    public string icon { get; set; } = null;
    public List<SkillCategoriesDetailDTO> children { get; set; }

    public SkillCategoriesDetailDTO()
    {
        children = new List<SkillCategoriesDetailDTO>();
    }
}
