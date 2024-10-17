﻿using Microsoft.AspNet.Identity;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IUserService
{
    IAsyncEnumerable<UserDTO> GetAll(int companyId);
    Task<UserDTO?> GetByIdAsync(int companyId, string id);
    Task<UserDTO?> GetByIdAsync(string id);
    Task<AspNetUser> AddUserAsync(AspNetUser entity);
    Task UpdateUserAsync(string id, UserDTO dto);
    Task DeleteUserAsync(string id);
    Task<UserDTO?> GetUserByName(string username);
    Task<AspNetUser> AddUserPasswordAsync(AspNetUser user, string password);
    Task<bool> SignInAsync(string username, string password);
    Task UpdatePasswordAsync(AspNetUser user, string newPassword);
}
