using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Interfaces;

public interface IUserRepository : IRepository<AspNetUser>
{
    Task<AspNetUser?> GetByUserName(string username);
    Task<UserDTO?> GetUserByName(string username);
}
