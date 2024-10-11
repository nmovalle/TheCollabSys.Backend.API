using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapperService<RoleDTO, AspNetRole> _mapperService;
    public RoleService(IUnitOfWork unitOfWork ,IMapperService<RoleDTO, AspNetRole> mapperService)
    {
        _unitOfWork = unitOfWork;
        _mapperService = mapperService;
    }

    public async Task<IEnumerable<RoleDTO>> GetAllRolesAsync()
    {
        var roles = await _unitOfWork.RoleRepository.GetAllAsync();
        var dto = roles.Select(_mapperService.MapToSource).ToList();

        return dto;
    }

    public async Task<RoleDTO?> GetRoleByIdAsync(string id)
    {
        var resp = await _unitOfWork.RoleRepository.GetAllQueryable()
            .Where(c => c.Id == id)
            .Select(c => new RoleDTO
            {
             Id = c.Id,
             Name = c.Name,
             NormalizedName = c.NormalizedName
            })
            .FirstOrDefaultAsync();

        return resp;
    }

    public async Task<RoleDTO?> GetRoleByNameAsync(string name)
    {
        return await _unitOfWork.RoleRepository.GetRoleByNameAsync(name);
    }
}
