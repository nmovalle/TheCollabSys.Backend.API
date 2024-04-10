using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public interface IClientService
{
    Task<DdClient> CreateClientAsync(DdClient clientEntity);
    Task<IEnumerable<ClientDTO>> GetAllClientsAsync();
    Task<ClientDTO?> GetClientByIdAsync(int id);
    Task UpdateClientAsync(int id, ClientDTO clientDTO);
    Task DeleteClientAsync(int id);

    Task<IEnumerable<ClientDTO>> GetClientsByNameAsync(string name);
}
