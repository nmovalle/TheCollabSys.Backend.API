using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IWireListService
{
    IAsyncEnumerable<WireListDTO> GetAll();
    Task<WireListDTO?> GetByIdAsync(int id);
    Task<DdWireList> Create(DdWireList entity);
    Task Update(int id, WireListDTO dto);
    Task Delete(int id);
    Task<WireListDTO?> GetByEmail(string email);
}
