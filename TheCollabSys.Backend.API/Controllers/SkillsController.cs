using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class SkillsController : BaseController
    {
        private readonly ISkillService _service;
        private readonly IMapperService<SkillDTO, DdSkill> _mapper;
        public SkillsController(
            ISkillService service,
            IMapperService<SkillDTO, DdSkill> mapperService
            )
        {
            _service = service;
            _mapper = mapperService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSkills()
        {
            return await ExecuteAsync(async () =>
            {
                var ienumerable = _service.GetAll();
                var data = new List<SkillDTO>();

                await foreach (var item in ienumerable)
                {
                    data.Add(item);
                }

                if (data.Any())
                {
                    return CreateResponse("success", data, "success");
                }

                return CreateNotFoundResponse<object>(null, "Register not found");
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSkillById(int id)
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
        public async Task<IActionResult> CreateSkill([FromForm] string dto)
        {
            return await this.HandleClientOperationAsync<SkillDTO>(dto, null, async (model) =>
            {
                var entity = _mapper.MapToDestination(model);
                var savedEntity = await _service.Create(entity);
                return CreateResponse("success", savedEntity, "success");
            });
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateSkill(int id, [FromForm] string dto)
        {
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
                return NotFound("Register not found");

            return await HandleClientOperationAsync<SkillDTO>(dto, null, async (model) =>
            {
                await _service.Update(id, model);
                return NoContent();
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSkill(int id)
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
