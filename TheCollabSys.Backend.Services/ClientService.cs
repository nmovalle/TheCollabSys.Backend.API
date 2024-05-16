using AutoMapper;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class ClientService : IClientService 
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMapperService<ClientDTO, DdClient> _clientMapper;

    public ClientService(IUnitOfWork unitOfWork, IMapper mapper, IMapperService<ClientDTO, DdClient> clientMapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _clientMapper = clientMapper;
    }

    public async Task<IEnumerable<ClientDTO>> GetAllClientsAsync()
    {
        var clients = await _unitOfWork.Clients.GetAllAsync();
        var clientsDTO = clients.Select(_clientMapper.MapToSource).ToList();

        return clientsDTO;
    }

    public async Task<ClientDTO?> GetClientByIdAsync(int id)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(id);
        return _clientMapper.MapToSource(client);
    }

    public async Task<DdClient> CreateClientAsync(DdClient clientEntity)
    {
        clientEntity.DateCreated = DateTime.Now;
        _unitOfWork.Clients.Add(clientEntity);
        await _unitOfWork.CompleteAsync();
        return clientEntity;
    }

    public async Task UpdateClientAsync(int id, ClientDTO clientDTO)
    {
        var existingClient = await _unitOfWork.Clients.GetByIdAsync(id);
        if (existingClient == null)
        {
            throw new ArgumentException("Client not found");
        }

        clientDTO.DateUpdate = DateTime.Now;

        var excludeProperties = new List<string> { "DateCreated" };
        _clientMapper.Map(clientDTO, existingClient, excludeProperties);

        _unitOfWork.Clients.Update(existingClient);

        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteClientAsync(int id)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(id);
        if (client == null)
        {
            throw new ArgumentException("Client not found");
        }

        _unitOfWork.Clients.Remove(client);
        await _unitOfWork.CompleteAsync();
    }

    public async Task<IEnumerable<ClientDTO>> GetClientsByNameAsync(string name)
    {
        var clients = await _unitOfWork.Clients.GetClientsByNameAsync(name);
        var clientsDTO = clients.Select(_clientMapper.MapToSource).ToList();

        return clientsDTO;
    }
}
