using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IClientService
{
    IAsyncEnumerable<ClientDTO> GetAllClientsAsync(int companyId);
    Task<IQueryable<ClientDTO>> GetClientByIdAsync(int companyId,int id);
    Task<IEnumerable<ClientDTO>> GetClientsByNameAsync(int companyId, string name);
    Task<DdClient> CreateClientAsync(DdClient clientEntity);
    Task UpdateClientAsync(int id, ClientDTO clientDTO);
    Task DeleteClientAsync(int id);
}
