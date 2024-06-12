using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class EmployersController : BaseController
    {
        private readonly IEmployerService _service;
        private readonly IMapperService<EmployerDTO, DdEmployer> _mapper;
        public EmployersController(
            IEmployerService employerService,
            IMapperService<EmployerDTO, DdEmployer> mapperService
            )
        {
            _service = employerService;
            _mapper = mapperService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployers()
        {
            return await ExecuteAsync(async () =>
            {
                var data = await _service.GetAll().ToListAsync();

                if (data.Any())
                    return CreateResponse("success", data, "success");

                return CreateNotFoundResponse<object>(null, "Registers not founds");
            });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetEmployerById(int id)
        {
            return await ExecuteAsync(async () =>
            {
                var data = await _service.GetByIdAsync(id);

                if (data == null)
                    return CreateNotFoundResponse<object>(null,"register not found");

                return CreateResponse("success", data, "success");
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployer([FromForm] string dto, [FromForm] IFormFile? file)
        {
            return await this.HandleClientOperationAsync<EmployerDTO>(dto, file, async (model) =>
            {
                var entity = _mapper.MapToDestination(model);
                var savedEntity = await _service.Create(entity);
                return CreateResponse("success", savedEntity, "success");
            });
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateEmployer(int id, [FromForm] string dto, [FromForm] IFormFile? file)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return CreateNotFoundResponse<object>(null,"register not found");

            return await this.HandleClientOperationAsync<EmployerDTO>(dto, file, async (model) =>
            {
                await _service.Update(id, model);
                return NoContent();
            });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteEmployer(int id)
        {
            try
            {
                await _service.Delete(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return CreateNotFoundResponse<object>(null, "register not found");
            }
        }
    }
}
