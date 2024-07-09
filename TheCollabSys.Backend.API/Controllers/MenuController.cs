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
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(GlobalExceptionFilter))]
    [ServiceFilter(typeof(ModelStateFilter))]
    public class MenuController : BaseController
    {
        private readonly IMenuService _service;
        private readonly IMapperService<MenuDTO, DdMenu> _mapper;
        public MenuController(
            IMenuService service,
            IMapperService<MenuDTO, DdMenu> mapperService
            )
        {
            _service = service;
            _mapper = mapperService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMenus()
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
        public async Task<IActionResult> GetMenuById(int id)
        {
            return await ExecuteAsync(async () =>
            {
                var data = await _service.GetByIdAsync(id);

                if (data == null)
                    return CreateNotFoundResponse<object>(null, "register not found");

                return CreateResponse("success", data, "success");
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenu([FromForm] string dto)
        {
            return await HandleClientOperationAsync<MenuDTO>(dto, null, async (model) =>
            {
                var entity = _mapper.MapToDestination(model);
                var savedEntity = await _service.Create(entity);
                return CreateResponse("success", savedEntity, "success");
            });
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateMenu(int id, [FromForm] string dto)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound("Register not found");

            return await HandleClientOperationAsync<MenuDTO>(dto, null, async (model) =>
            {
                await _service.Update(id, model);
                return NoContent();
            });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteMenu(int id)
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
