using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Interfaces;

public interface IUserRepository : IRepository<AspNetUser>
{
    Task<UserDTO?> GetUserByName(string username);
}
