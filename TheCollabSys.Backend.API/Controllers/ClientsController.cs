using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TheCollabSys.Backend.API.Filters;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Controllers
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(GlobalExceptionFilter))]
    [ServiceFilter(typeof(ModelStateFilter))]
    [ServiceFilter(typeof(UserIdFilter))]
    public class ClientsController : BaseController
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

        [HttpGet]
        [ActionName(nameof(GetAllClientsAsync))]
        public async Task<IActionResult> GetAllClientsAsync()
        {
            return await ExecuteAsync(async () =>
            {
                var clientEnumerable = _clientService.GetAllClientsAsync();
                var clients = new List<ClientDTO>();

                await foreach (var client in clientEnumerable)
                {
                    clients.Add(client);
                }

                if (clients.Any())
                {
                    return CreateResponse("success", clients, "success");
                }

                return CreateNotFoundResponse("error", "Register not found");
            });
        }

        [HttpGet("{id}")]
        [ActionName(nameof(GetClientByIdAsync))]
        public async Task<IActionResult> GetClientByIdAsync(int id)
        {
            return await ExecuteAsync(async () =>
            {
                var queryableData = await _clientService.GetClientByIdAsync(id);
                var data = await queryableData.ToListAsync();

                if (data == null || !data.Any())
                    return CreateNotFoundResponse<object>(null, "register was not found");

                return CreateResponse("success", data[0], "success");
            });
        }


        [HttpGet("SearchClientsByName/{name}", Name = "SearchClientsByName")]
        [ActionName(nameof(SearchClientsByName))]
        public async Task<IActionResult> SearchClientsByName(string name)
        {
            var clients = await _clientService.GetClientsByNameAsync(name);
            return Ok(clients);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromForm] string clientDTO, [FromForm] IFormFile? file)
        {
            return await HandleClientOperationAsync(clientDTO, file, async (model) =>
            {
                var clientEntity = _clientMapper.MapToDestination(model);
                var savedClientEntity = await _clientService.CreateClientAsync(clientEntity);
                var savedClientDTO = _clientMapper.MapToSource(savedClientEntity);
                return CreatedAtAction(nameof(GetClientByIdAsync), new { id = savedClientDTO.ClientID }, savedClientDTO);
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromForm] string clientDTO, [FromForm] IFormFile? file)
        {
            var existingClient = await _clientService.GetClientByIdAsync(id);
            if (existingClient == null)
            {
                return NotFound();
            }

            return await HandleClientOperationAsync(clientDTO, file, async (model) =>
            {
                await _clientService.UpdateClientAsync(id, model);
                return NoContent();
            });
        }

        private async Task<IActionResult> HandleClientOperationAsync(string clientDTO, IFormFile? file, Func<ClientDTO, Task<IActionResult>> clientOperation)
        {
            try
            {
                var userId = HttpContext.Request.Headers["User-Id"];
                var model = JsonConvert.DeserializeObject<ClientDTO>(clientDTO);
                if (model == null)
                {
                    return BadRequest("Invalid client data");
                }

                if (file != null && file.Length > 0)
                {
                    (string fileType, byte[] fileBytes) = await ProcessFileAsync(file);
                    model.Filetype = fileType;
                    model.Logo = fileBytes;
                }

                model.UserId = userId;
                return await clientOperation(model);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the client");
            }
        }

        private async Task<(string fileType, byte[] fileBytes)> ProcessFileAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var fileBytes = memoryStream.ToArray();
                var fileType = file.ContentType;
                return (fileType, fileBytes);
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
