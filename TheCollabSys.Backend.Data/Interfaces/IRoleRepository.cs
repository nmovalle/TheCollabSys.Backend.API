using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Interfaces;

public interface IRoleRepository : IRepository<AspNetRole>
{
    Task<RoleDTO?> GetRoleByNameAsync(string name);
}
