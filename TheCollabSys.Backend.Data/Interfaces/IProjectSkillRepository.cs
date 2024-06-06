using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Interfaces;

public interface IProjectSkillRepository : IRepository<DdProjectSkill>
{
    Task<IEnumerable<DdProjectSkill>> GetSkillsByProjectIdAsync(int projectId);
}
