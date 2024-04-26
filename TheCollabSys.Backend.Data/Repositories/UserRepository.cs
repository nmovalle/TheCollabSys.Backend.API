using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class UserRepository : Repository<AspNetUser>, IUserRepository
{
    public UserRepository(TheCollabsysContext context) : base(context)
    {
    }

    public async Task<AspNetUser?> GetByUserName(string username)
    {
        return await _context.AspNetUsers.FirstOrDefaultAsync(u => u.UserName == username);
    }
    public async Task<UserDTO?> GetUserByName(string? username)
    {
        return await _context.AspNetUsers
            .Select(u => new UserDTO
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email
            })
            .FirstOrDefaultAsync(u => u.UserName == username);
    }
}
