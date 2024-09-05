using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TheCollabSys.Backend.API.Extensions;
using TheCollabSys.Backend.API.Filters;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Models;
using TheCollabSys.Backend.Entity.Request;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Controllers;

//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(GlobalExceptionFilter))]
[ServiceFilter(typeof(ModelStateFilter))]
public class AccessCodeController : BaseController
{
    private readonly IAccessCodeService _service;
    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IMapperService<AccessCodeDTO, DdAccessCode> _mapper;
    public AccessCodeController(
        IAccessCodeService service,
        IConfiguration configuration,
        IEmailService emailService,
        IMapperService<AccessCodeDTO, DdAccessCode> mapperService
        )
    {
        _service = service;
        _configuration = configuration;
        _emailService = emailService;
        _mapper = mapperService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
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
    public async Task<IActionResult> GetById(int id)
    {
        return await ExecuteAsync(async () =>
        {
            var data = await _service.GetByIdAsync(id);

            if (data == null)
                return CreateNotFoundResponse<object>(null, "register not found");

            return CreateResponse("success", data, "success");
        });
    }

    [HttpGet]
    [Route("GetByEmail/{id}")]
    public async Task<IActionResult> GetByEmail(string email)
    {
        return await ExecuteAsync(async () =>
        {
            var data = await _service.GetByEmail(email);

            if (data == null)
                return CreateNotFoundResponse<object>(null, "register not found");

            return CreateResponse("success", data, "success");
        });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] string dto)
    {
        return await this.HandleClientOperationAsync<AccessCodeDTO>(dto, null, async (model) =>
        {
            var entity = _mapper.MapToDestination(model);
            var savedEntity = await _service.Create(entity);
            return CreateResponse("success", savedEntity, "success");
        });
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] string dto)
    {
        var existing = await _service.GetByIdAsync(id);
        if (existing == null)
            return NotFound("Register not found");

        return await HandleClientOperationAsync<AccessCodeDTO>(dto, null, async (model) =>
        {
            await _service.Update(id, model);
            return NoContent();
        });
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete(int id)
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

    [HttpPost]
    [Route("generate")]
    public async Task<IActionResult> GenerateAccessCode([FromBody] EmailDTO emailDto)
    {
        return await this.HandleClientOperationAsync<EmailDTO>(JsonConvert.SerializeObject(emailDto), null, async (emailInput) =>
        {
            var existingAccessCode = await _service.GetByEmail(emailInput.Email);
            if (existingAccessCode != null)
                return CreateNotFoundResponse<object>(null, $"There is already a valid access code for {emailInput.Email}");

            // Generar un código de acceso de 6 dígitos aleatorios
            int expireMinutes = _configuration.GetValue<int>("AccessCode:ExpireMinutes");
            var random = new Random();
            var accessCode = random.Next(100000, 999999).ToString();
            var currentTime = DateTime.UtcNow;

            var ddAccessCode = new DdAccessCode
            {
                Email = emailInput.Email,
                AccessCode = accessCode,
                RegAt = DateTime.UtcNow,
                ExpAt = currentTime.AddMinutes(expireMinutes)
            };

            var savedEntity = await _service.Create(ddAccessCode);
            var accessCodeDTO = _mapper.MapToSource(savedEntity);

            // Enviar el código de acceso por correo electrónico
            var subject = "Your Access Code";
            var body = $"<p>Your access code is: <strong>{accessCode}</strong></p>";
            await _emailService.SendEmailAsync(emailInput.Email, subject, body);

            return CreateResponse("success", accessCodeDTO, "We have sent an email with your access code.");
        });
    }

    [HttpPost]
    [Route("resend")]
    public async Task<IActionResult> ResendAccessCode([FromBody] EmailDTO emailDto)
    {
        return await this.HandleClientOperationAsync<EmailDTO>(JsonConvert.SerializeObject(emailDto), null, async (emailInput) =>
        {
            var existingAccessCode = await _service.GetByEmail(emailInput.Email);

            // Generar un nuevo código de acceso de 6 dígitos aleatorios
            int expireMinutes = _configuration.GetValue<int>("AccessCode:ExpireMinutes");
            var random = new Random();
            var newAccessCode = random.Next(100000, 999999).ToString();
            var currentTime = DateTime.UtcNow;

            if (existingAccessCode != null)
            {
                // Actualizar el código de acceso y extender su tiempo de expiración
                existingAccessCode.AccessCode = newAccessCode;
                existingAccessCode.RegAt = currentTime;
                existingAccessCode.ExpAt = currentTime.AddMinutes(expireMinutes);
                existingAccessCode.IsValid = false;

                await _service.Update(existingAccessCode.Id, existingAccessCode);

                // Enviar el nuevo código de acceso por correo electrónico
                var subject = "Your New Access Code";
                var body = $"<p>Your new access code is: <strong>{newAccessCode}</strong></p>";
                await _emailService.SendEmailAsync(emailInput.Email, subject, body);

                return CreateResponse("success", newAccessCode, "We have sent an email with your access code.");
            }
            else
            {
                // Crear un nuevo código de acceso
                var ddAccessCode = new DdAccessCode
                {
                    Email = emailInput.Email,
                    AccessCode = newAccessCode,
                    RegAt = currentTime,
                    ExpAt = currentTime.AddMinutes(expireMinutes),
                    IsValid = false
                };

                var savedEntity = await _service.Create(ddAccessCode);
                var accessCodeDTO = _mapper.MapToSource(savedEntity);

                // Enviar el nuevo código de acceso por correo electrónico
                var subject = "Your New Access Code";
                var body = $"<p>Your new access code is: <strong>{newAccessCode}</strong></p>";
                await _emailService.SendEmailAsync(emailInput.Email, subject, body);

                return CreateResponse("success", newAccessCode, "We have sent an email with your access code.");
            }
        });
    }


    [HttpPost]
    [Route("validate")]
    public async Task<IActionResult> ValidateAccessCode([FromBody] ValidateAccessCodeRequest request)
    {
        return await this.HandleClientOperationAsync<ValidateAccessCodeRequest>(JsonConvert.SerializeObject(request), null, async (emailInput) =>
        {
            var accessCode = await _service.GetByAccessCodeEmail(emailInput.Code, emailInput.Email);

            if (accessCode == null || accessCode.ExpAt < DateTime.UtcNow)
                return NotFound(new { AccessCode = request.Code, Email = request.Email, Message = "Invalid or expired access code" });

            if ((bool)accessCode.IsValid)
                return NotFound(new { AccessCode = request.Code, Email = request.Email, Message = "Access code has already been used" });

            accessCode.IsValid = true;
            await _service.Update(accessCode.Id, accessCode);

            return CreateResponse("success", accessCode, "Access code is valid");
        });
    }
}
