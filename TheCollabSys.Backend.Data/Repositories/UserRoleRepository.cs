using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data.Repositories;

public class UserRoleRepository : Repository<AspNetUserRole>, IUserRoleRepository
{
    public UserRoleRepository(TheCollabsysContext context) : base(context)
    {
    }

    public async Task<UserRoleDTO> GetUserRoleByUserName(string username)
    {
        var userRole = await _context.AspNetUserRoles
        .Include(ur => ur.Role)
        .FirstOrDefaultAsync(u => u.User.UserName == username);

        if (userRole == null)
        {
            return null; // El usuario no existe o no tiene ningún rol asignado
        }

        return new UserRoleDTO
        {
            UserId = userRole.UserId,
            RoleId = userRole.RoleId,
            RoleName = userRole.Role.Name
        };
    }

    public async Task<UserRoleDTO> UpdateUserRoleByUserName(string username, string newRoleId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Buscar el usuario y su rol asociado
            var userRole = await _context.AspNetUserRoles
                .Include(ur => ur.Role) // Incluir la entidad Role
                .FirstOrDefaultAsync(u => u.User.UserName == username);

            // Verificar si el usuario existe y tiene un rol asignado
            if (userRole == null)
            {
                return null; // El usuario no existe o no tiene ningún rol asignado
            }

            // Guardar el Id del usuario antes de eliminar la entrada existente
            string userId = userRole.UserId;

            // Guardar el nombre del rol antes de eliminar la entrada existente
            string roleNameBeforeUpdate = userRole.Role?.Name;

            // Eliminar la entrada existente de AspNetUserRoles
            _context.AspNetUserRoles.Remove(userRole);

            // Crear una nueva entrada de AspNetUserRole con el nuevo RoleId
            var newUserRole = new AspNetUserRole
            {
                UserId = userId, // Utilizar el Id del usuario guardado
                RoleId = newRoleId
            };

            // Agregar la nueva entrada a AspNetUserRoles
            _context.AspNetUserRoles.Add(newUserRole);

            // Guardar los cambios en la base de datos
            await _context.SaveChangesAsync();

            // Commit de la transacción
            await transaction.CommitAsync();

            // Obtener el nombre del nuevo rol después de agregar la nueva entrada
            var newRole = await _context.AspNetRoles.FirstOrDefaultAsync(r => r.Id == newRoleId);
            string roleNameAfterUpdate = newRole?.Name;

            // Crear un DTO con la información actualizada
            return new UserRoleDTO
            {
                UserId = userId, // Utilizar el Id del usuario guardado
                RoleId = newRoleId,
                RoleName = roleNameAfterUpdate ?? roleNameBeforeUpdate // Devolver el nombre del nuevo rol si está disponible, de lo contrario, devolver el nombre del rol antes de la actualización
            };
        }
        catch (Exception)
        {
            // En caso de error, hacer rollback de la transacción
            await transaction.RollbackAsync();
            throw; // Relanzar la excepción para que sea manejada en un nivel superior
        }
    }
}
