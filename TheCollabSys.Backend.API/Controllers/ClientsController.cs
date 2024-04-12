using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.API.Filters;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Controllers
{

    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(GlobalExceptionFilter))]
    [ServiceFilter(typeof(ModelStateFilter))]
    public class ClientsController : ControllerBase
    {
        private readonly ILogger<ClientsController> _logger;
        private readonly IClientService _clientService;
        private readonly IMapperService<ClientDTO, DdClient> _clientMapper;

        public ClientsController(ILogger<ClientsController> logger, IClientService clientService, IMapperService<ClientDTO, DdClient> clientMapper)
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

        [HttpPut("{id}")]      
        public async Task<IActionResult> UpdateClient(int id, [FromBody] ClientDTO clientDTO)
        {
          try
            {
                    // Directly await the asynchronous method without assigning its result to a variable
              await _clientService.UpdateClientAsync(id, clientDTO);
              return NoContent();
            }
                catch (ArgumentException ex)
                {
                    return NotFound(ex.Message);
                }
                catch (Exception ex)
                {
                    // Log the exception
                    return StatusCode(500, "An error occurred while updating the client");
                }
         } 
        // Delete client
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientAsync(int id)
        {
            try
            {
                await _clientService.DeleteClientAsync(id);
                return NoContent(); // 204 No Content for successful deletion
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message); // 404 Not Found if the client does not exist
            }
        }
        
    }
}
