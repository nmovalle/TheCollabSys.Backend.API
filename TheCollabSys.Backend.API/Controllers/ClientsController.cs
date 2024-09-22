using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TheCollabSys.Backend.API.Extensions;
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
            return await ExecuteWithCompanyIdAsync(async (companyId) =>
            {
                var data = await _clientService.GetAllClientsAsync(companyId).ToListAsync();

                if (data.Any())
                    return CreateResponse("success", data, "success");

                return CreateNotFoundResponse<object>(null, "Registers not found");
            });
        }

        [HttpGet]
        [Route("{id}")]
        [ActionName(nameof(GetClientByIdAsync))]
        public async Task<IActionResult> GetClientByIdAsync(int id)
        {
            return await ExecuteWithCompanyIdAsync(async (companyId) =>
            {
                var queryableData = await _clientService.GetClientByIdAsync(companyId,id);
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
            return await ExecuteWithCompanyIdAsync(async (companyId) =>
            {
                var data = await _clientService.GetClientsByNameAsync(companyId, name);

                if (data == null)
                    return CreateNotFoundResponse<object>(null, "register was not found");

                return CreateResponse("success", data, "success");
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateClient([FromForm] string clientDTO, [FromForm] IFormFile? file)
        {
            return await this.HandleClientOperationAsync<ClientDTO>(clientDTO, file, async (model) =>
            {
                var entity = _clientMapper.MapToDestination(model);
                var savedEntity = await _clientService.CreateClientAsync(entity);
                return CreateResponse("success", savedEntity, "success");
            });
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateClient(int id, [FromForm] string clientDTO, [FromForm] IFormFile? file)
        {
            return await this.HandleClientOperationAsync<ClientDTO>(clientDTO, file, async (model) =>
            {
                if (model.CompanyId == null) return NotFound("Company Id is missing.");

                var existingClient = await _clientService.GetClientByIdAsync((int)model.CompanyId, id);
                if (existingClient == null) return NotFound();


                await _clientService.UpdateClientAsync(id, model);
                return NoContent();
            });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteClientAsync(int id)
        {
            try
            {
                await _clientService.DeleteClientAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
