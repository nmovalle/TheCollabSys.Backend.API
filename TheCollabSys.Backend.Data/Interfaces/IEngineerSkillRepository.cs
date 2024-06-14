using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Interfaces;

public interface IEngineerSkillRepository : IRepository<DdEngineerSkill>
{
    Task<IEnumerable<DdEngineerSkill>> GetSkillsByEngineerIdAsync(int engineerId);
}
