using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class RoleRepository : Repository<AspNetRole>, IRoleRepository
{
    public RoleRepository(TheCollabsysContext context) : base(context)
    {
    }

    public async Task<RoleDTO?> GetRoleByNameAsync(string name)
    {
        return await _context.AspNetRoles
            .Select(r => new RoleDTO
            {
                Id = r.Id,
                Name = r.Name,
                NormalizedName = r.NormalizedName
            })
            .FirstOrDefaultAsync(r => r.Name == name);
    }
}
