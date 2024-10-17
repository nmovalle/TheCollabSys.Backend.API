using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<UserDTO, AspNetUser> _userMapper;
    private readonly IPasswordHasher<AspNetUser> _passwordHasher;
    public UserService(IUnitOfWork unitOfWork, IMapperService<UserDTO, AspNetUser> userMapper, IPasswordHasher<AspNetUser> passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _userMapper = userMapper;
        _passwordHasher = passwordHasher;
    }

    public IAsyncEnumerable<UserDTO> GetAll(int companyId)
    {
        var data = (from us in _unitOfWork.UserRepository.GetAllQueryable()
                    join c in _unitOfWork.UserCompanyRepository.GetAllQueryable() on us.Id equals c.UserId
                    where c.CompanyId == companyId
                    group us by us.Id into grouped
                    select new UserDTO
                    {
                        Id = grouped.Key,
                        UserName = grouped.Select(g => g.UserName).FirstOrDefault(),
                        Email = grouped.Select(g => g.Email).FirstOrDefault(),
                    }).AsAsyncEnumerable();

        return data;
    }

    public async Task<UserDTO?> GetByIdAsync(int companyId, string id)
    {
        var data = await (from us in _unitOfWork.UserRepository.GetAllQueryable()
                          join c in _unitOfWork.UserCompanyRepository.GetAllQueryable() on us.Id equals c.UserId
                          where c.CompanyId == companyId && us.Id == id
                          select new UserDTO
                          {
                              Id = us.Id,
                              UserName = us.UserName,
                              Email = us.Email,
                          }).FirstOrDefaultAsync();

        return data;
    }

    public async Task<UserDTO?> GetByIdAsync(string id)
    {
        var data = await (from us in _unitOfWork.UserRepository.GetAllQueryable()
                          join c in _unitOfWork.UserCompanyRepository.GetAllQueryable() on us.Id equals c.UserId
                          where us.Id == id
                          select new UserDTO
                          {
                              Id = us.Id,
                              UserName = us.UserName,
                              Email = us.Email,
                          }).FirstOrDefaultAsync();

        return data;
    }

    public async Task<AspNetUser> AddUserAsync(AspNetUser entity)
    {
        _unitOfWork.UserRepository.Add(entity);
        await _unitOfWork.CompleteAsync();
        return entity;
    }

    public async Task UpdateUserAsync(string id, UserDTO dto)
    {
        var existing = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (existing == null)
        {
            throw new ArgumentException("User not found");
        }

        _userMapper.Map(dto, existing);
        _unitOfWork.UserRepository.Update(existing);

        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteUserAsync(string id)
    {
        var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new ArgumentException("User not found");
        }

        _unitOfWork.UserRepository.Remove(user);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<UserDTO?> GetUserByName(string username)
    {
        return await _unitOfWork.UserRepository.GetUserByName(username);
    }

    public async Task<AspNetUser> AddUserPasswordAsync(AspNetUser user, string password)
    {
        var passwordHash = new PasswordHasher<AspNetUser>().HashPassword(user, password);
        var aspNetUser = new AspNetUser
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            PasswordHash = passwordHash
        };

        _unitOfWork.UserRepository.Add(aspNetUser);
        await _unitOfWork.CompleteAsync();
        return aspNetUser;
    }

    public async Task<bool> SignInAsync(string username, string password)
    {
        var user = await _unitOfWork.UserRepository.GetByUserName(username);
        if (user == null) return false;

        if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password) == PasswordVerificationResult.Success)
        {
            return true;
        }

        return false;
    }

    public async Task UpdatePasswordAsync(AspNetUser user, string newPassword)
    {
        var newPasswordHash = _passwordHasher.HashPassword(user, newPassword);
        var aspNetUser = await _unitOfWork.UserRepository.GetByUserName(user.UserName);
        if (aspNetUser != null)
        {
            aspNetUser.PasswordHash = newPasswordHash;
            await _unitOfWork.CompleteAsync();
        }
    }
}
