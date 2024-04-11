using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Interfaces;

public interface IUserRoleRepository : IRepository<AspNetUserRole>
{
    Task<UserRoleDTO> GetUserRoleByUserName(string username);
    Task<UserRoleDTO> UpdateUserRoleByUserName(string username, string newRoleId);
}
