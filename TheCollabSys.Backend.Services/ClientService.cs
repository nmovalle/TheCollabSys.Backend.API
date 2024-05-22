using AutoMapper;
using TheCollabSys.Backend.Data;
using TheCollabSys.Backend.Data.Interfaces;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Services;

public class ClientService : IClientService 
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IMapperService<ClientDTO, DdClient> _clientMapper;
    private readonly TheCollabsysContext _context;

    public ClientService(TheCollabsysContext context, IUnitOfWork unitOfWork, IMapper mapper, IMapperService<ClientDTO, DdClient> clientMapper)
    {
        _context = context;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _clientMapper = clientMapper;
    }

    public IAsyncEnumerable<ClientDTO> GetAllClientsAsync()
    {
        return _unitOfWork.Clients.GetAllProjectedAsAsyncEnumerable(client => new ClientDTO
        {
            ClientID = client.ClientId,
            ClientName = client.ClientName,
            Address = client.Address,
            Phone = client.Phone,
            Email = client.Email,
            Logo = client.Logo,
            Filetype = client.Filetype,
            Active = client.Active,
            DateCreated = client.DateCreated,
            DateUpdate = client.DateUpdate,
            UserId = client.UserId,
        });
    }

    public Task<IQueryable<ClientDTO>> GetClientByIdAsync(int id)
{
    var queryableData = _context.DD_Clients
        .Where(c => c.ClientId == id)
        .Select(client => new ClientDTO
        {
            ClientID = client.ClientId,
            ClientName = client.ClientName,
            Address = client.Address,
            Phone = client.Phone,
            Email = client.Email,
            Logo = client.Logo,
            Filetype = client.Filetype,
            Active = client.Active,
            DateCreated = client.DateCreated,
            DateUpdate = client.DateUpdate,
            UserId = client.UserId
        });

    return Task.FromResult(queryableData);
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
            throw new ArgumentException("Client not found");

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
