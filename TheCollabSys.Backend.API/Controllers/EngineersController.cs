﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheCollabSys.Backend.API.Extensions;
using TheCollabSys.Backend.API.Filters;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
[ServiceFilter(typeof(GlobalExceptionFilter))]
[ServiceFilter(typeof(ModelStateFilter))]
[ServiceFilter(typeof(UserIdFilter))]
public class EngineersController : BaseController
{
    private readonly IEngineerService _service;
    private readonly IMapperService<EngineerDTO, DdEngineer> _mapper;
    public EngineersController(
        IEngineerService service,
        IMapperService<EngineerDTO, DdEngineer> mapperService
        )
    {
        _service = service;
        _mapper = mapperService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllEngineers()
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
    public async Task<IActionResult> GetEngineerById(int id)
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
    public async Task<IActionResult> CreateEngineer([FromForm] string dto, [FromForm] IFormFile? file)
    {
        return await this.HandleClientOperationAsync<EngineerDTO>(dto, file, async (model) =>
        {
            var entity = _mapper.MapToDestination(model);
            var savedEntity = await _service.Create(entity);
            return CreateResponse("success", savedEntity, "success");
        });
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> UpdateEngineer(int id, [FromForm] string dto, [FromForm] IFormFile? file)
    {
        var existing = await _service.GetByIdAsync(id);
        if (existing == null)
            return CreateNotFoundResponse<object>(null, "register not found");

        return await this.HandleClientOperationAsync<EngineerDTO>(dto, file, async (model) =>
        {
            await _service.Update(id, model);
            return NoContent();
        });
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> DeleteEngineer(int id)
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