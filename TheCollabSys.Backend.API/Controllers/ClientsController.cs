using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.API.Filters;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(GlobalExceptionFilter))]
    [ServiceFilter(typeof(ModelStateFilter))]
    public class ClientsController : ControllerBase
    {
        private readonly ILogger<ClientsController> _logger;
        private readonly IClientService _clientService;
        private readonly IMapperService<ClientDTO, DD_Clients> _clientMapper;

        public ClientsController(ILogger<ClientsController> logger, IClientService clientService, IMapperService<ClientDTO, DD_Clients> clientMapper)
        {
            _logger = logger;
            _clientService = clientService;
            _clientMapper = clientMapper;
        }

        [HttpGet("GetAllClientsAsync", Name = "GetAllClientsAsync")]
        [ActionName(nameof(GetAllClientsAsync))]
        public async Task<IActionResult> GetAllClientsAsync()
        {
            return Ok(await _clientService.GetAllClientsAsync());
        }

        [HttpGet("GetClientByIdAsync/{id}", Name = "GetClientByIdAsync")]
        [ActionName(nameof(GetClientByIdAsync))]
        public async Task<IActionResult> GetClientByIdAsync(int id)
        {
            var client = await _clientService.GetClientByIdAsync(id);
            if (client == null)
                return NotFound();

            return Ok(client);
        }

        [HttpGet("SearchClientsByName/{name}", Name = "SearchClientsByName")]
        [ActionName(nameof(SearchClientsByName))]
        public async Task<IActionResult> SearchClientsByName(string name)
        {
            var clients = await _clientService.GetClientsByNameAsync(name);
            return Ok(clients);
        }

        [HttpPost]
        [ActionName(nameof(CreateClientAsync))]
        public async Task<IActionResult> CreateClientAsync(ClientDTO clientDTO)
        {
            var clientEntity = _clientMapper.MapToDestination(clientDTO);
            var savedClientEntity = await _clientService.CreateClientAsync(clientEntity);
            var savedClientDTO = _clientMapper.MapToSource(savedClientEntity);

            return CreatedAtAction(nameof(GetClientByIdAsync), new { id = savedClientDTO.ClientID }, savedClientDTO);
        }
    }
}
