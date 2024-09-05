using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Numerics;
using TheCollabSys.Backend.API.Extensions;
using TheCollabSys.Backend.API.Filters;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Enums;
using TheCollabSys.Backend.Entity.Models;
using TheCollabSys.Backend.Entity.Request;
using TheCollabSys.Backend.Entity.Response;
using TheCollabSys.Backend.Services;

namespace TheCollabSys.Backend.API.Controllers;

//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
[ServiceFilter(typeof(GlobalExceptionFilter))]
[ServiceFilter(typeof(ModelStateFilter))]
public class InvitationController : BaseController
{
    private readonly IInvitationService _service;
    private readonly IWireListService _wireListService;
    private readonly IUserService _userService;
    private readonly IUserRoleService _userRoleService;
    private readonly IUserCompanyService _userCompanyService;
    private readonly ICompanyService _companyService;

    private readonly IConfiguration _configuration;
    private readonly IEmailService _emailService;
    private readonly IMapperService<InvitationDTO, DdInvitation> _mapper;
    private readonly IMapperService<WireListDTO, DdWireList> _mapperWireList;
    public InvitationController(
        IInvitationService service,
        IWireListService wireListService,
        IUserService userService,
        IUserRoleService userRoleService,
        IUserCompanyService userCompanyService,
        ICompanyService companyService,

        IConfiguration configuration,
        IEmailService emailService,
        IMapperService<InvitationDTO, DdInvitation> mapperService,
        IMapperService<WireListDTO, DdWireList> mapperWireList
        )
    {
        _service = service;
        _wireListService = wireListService;
        _userService = userService;
        _userRoleService = userRoleService;
        _userCompanyService = userCompanyService;
        _companyService = companyService;

        _configuration = configuration;
        _emailService = emailService;
        _mapper = mapperService;
        _mapperWireList = mapperWireList;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return await ExecuteAsync(() =>
        {
            var data = _service.GetAll();

            if (data.Any())
                return Task.FromResult(CreateResponse("success", data, "success"));

            return Task.FromResult(CreateNotFoundResponse<object>(null, "Registers not founds"));
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

    [HttpPost]
    public async Task<IActionResult> Create([FromForm] string dto)
    {
        return await this.HandleClientOperationAsync<InvitationDTO>(dto, null, async (model) =>
        {
            var entity = _mapper.MapToDestination(model);
            var savedEntity = await _service.Create(entity);
            return CreateResponse("success", savedEntity, "success");
        });
    }

    [HttpPost]
    [Route("generate")]
    public async Task<IActionResult> GenerateInitiation([FromBody] InvitationRequest request)
    {
        var invitation = GenerateInvitation(request.Email, request.IsExternal);
        var savedInvitation = await CreateOrUpdateEntityAsync<InvitationDTO, DdInvitation>(
            request.Email,
            async (email) => await _service.GetByEmail(email),
            async (entity, newValues) =>
            {
                // Aplicar los valores nuevos a la entidad
                await UpdateInvitationAsync(entity, newValues);
                var dto = _mapper.MapToSource(entity);
                await _service.Update(entity.Id, dto);
            },
            async () =>
            {
                var entity = _mapper.MapToDestination(invitation);
                return await _service.Create(entity);
            },
            invitation, // Pasar toda la invitación para la actualización o creación
            _mapper // Pasar el mapper adecuado
        );

        // Crear o actualizar la lista de wire
        var wireListDto = new WireListDTO
        {
            Email = request.Email,
            Domain = request.Domain,
            IsExternal = request.IsExternal,
            RoleId = request.RoleId,
            IsBlackList = request.IsBlackList
        };
        await CreateOrUpdateEntityAsync<WireListDTO, DdWireList>(
            request.Email,
            async (email) => await _wireListService.GetByEmail(email),
            async (entity, newValues) =>
            {
                // Aplicar los valores nuevos a la entidad
                await UpdateWireListAsync(entity, newValues);
                var dto = _mapperWireList.MapToSource(entity);
                await _wireListService.Update(entity.Id, dto);
            },
            async () =>
            {
                var entity = _mapperWireList.MapToDestination(wireListDto);
                return await _wireListService.Create(entity);
            },
            wireListDto, // Pasar toda la lista de wire para la actualización o creación
            _mapperWireList // Pasar el mapper adecuado
        );

        // Enviar el correo de invitación
        await SendInvitationEmailAsync(request.Email, invitation.Token);

        return CreateResponse("success", savedInvitation, "success");
    }

    [HttpGet("validate")]
    public async Task<IActionResult> ValidateInvitation([FromQuery] Guid token)
    {
        try
        {
            var invitation = await VerifyAndUpdateInvitationStatus(token);
            var wireList = await _wireListService.GetByEmail(invitation.Email);

            if (wireList == null)
            {
                return NotFound("Wire List not found.");
            }

            var (user, success, password) = await UpdateOrCreateUser(invitation.Email, invitation.IsExternal, wireList.RoleId);

            if (!success || user == null)
            {
                return StatusCode(500, "Failed to update or create the user.");
            }

            try
            {
                var accessUrl = _configuration["EmailSettings:AccessUrl"];
                await SendWelcomeEmailAsync(invitation.Email, accessUrl, invitation.IsExternal ? password : null);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "User created, but failed to send welcome email.");
            }

            return CreateResponse("success", invitation, "Token is valid");
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] string dto)
    {
        var existing = await _service.GetByIdAsync(id);
        if (existing == null)
            return NotFound("Register not found");

        return await HandleClientOperationAsync<InvitationDTO>(dto, null, async (model) =>
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

    #region Métodos Auxiliares

    /// <summary>
    /// Método genérico para crear o actualizar una entidad.
    /// </summary>
    private async Task<TDto> CreateOrUpdateEntityAsync<TDto, TEntity>(
    string email,
    Func<string, Task<TDto?>> getByEmailAsync,
    Func<TEntity, TDto, Task> updateAsync,
    Func<Task<TEntity>> createAsync,
    TDto newValues, // Pasar los nuevos valores directamente
    IMapperService<TDto, TEntity> mapper
) where TDto : class
    {
        var existingDto = await getByEmailAsync(email);

        if (existingDto != null)
        {
            // Convertir el DTO existente a una entidad
            var existingEntity = mapper.MapToDestination(existingDto);

            // Aplicar los nuevos valores a la entidad existente
            await updateAsync(existingEntity, newValues);

            return existingDto;
        }
        else
        {
            var newEntity = await createAsync();
            return mapper.MapToSource(newEntity);
        }
    }

    /// <summary>
    /// Método genérico para enviar correos electrónicos.
    /// </summary>
    private async Task SendEmailAsync(string email, string subject, string body)
    {
        await _emailService.SendEmailAsync(email, subject, body);
    }

    /// <summary>
    /// Método para construir el cuerpo del correo electrónico utilizando plantillas.
    /// </summary>
    private string BuildEmailBody(string template, params (string Placeholder, string Value)[] replacements)
    {
        foreach (var replacement in replacements)
        {
            template = template.Replace(replacement.Placeholder, replacement.Value);
        }
        return template;
    }

    /// <summary>
    /// Construye el cuerpo del correo de invitación.
    /// </summary>
    private string GetInvitationEmailBody(string invitationUrl, Guid token)
    {
        var template = "<p>Please use the following link to validate your invitation:</p><p><a href=\"{InvitationUrl}/{Token}\">Get Started</a></p>";
        return BuildEmailBody(template, ("{InvitationUrl}", invitationUrl), ("{Token}", token.ToString()));
    }

    /// <summary>
    /// Construye el cuerpo del correo de bienvenida.
    /// </summary>
    private string GetWelcomeEmailBody(string accessUrl, string email, string? password)
    {
        if (!string.IsNullOrEmpty(password))
        {
            var template = @"
                    <p>Welcome to The Collabsys!</p>
                    <p>Your account has been successfully created. Below are your login details:</p>
                    <p><strong>Username:</strong> {Email}</p>
                    <p><strong>Password:</strong> {Password}</p>
                    <p>You can access the system using the following link:</p>
                    <p><a href=""{AccessUrl}"">Login to The Collabsys</a></p>
                    <p>Please change your password after your first login.</p>";
            return BuildEmailBody(template, ("{AccessUrl}", accessUrl), ("{Email}", email), ("{Password}", password));
        }
        else
        {
            var template = @"
                    <p>Welcome to The Collabsys!</p>
                    <p>Your account has been successfully created.</p>
                    <p>You can access the system using the following link:</p>
                    <p><a href=""{AccessUrl}"">Login to The Collabsys</a></p>
                    <p>Thank you for joining us!</p>";
            return BuildEmailBody(template, ("{AccessUrl}", accessUrl));
        }
    }

    /// <summary>
    /// Envía el correo de invitación.
    /// </summary>
    private async Task SendInvitationEmailAsync(string email, Guid token)
    {
        var invitationUrl = _configuration["EmailSettings:InvitationUrl"];
        var body = GetInvitationEmailBody(invitationUrl, token);
        var subject = "Welcome to Collabsys - Activate Your Account";
        await SendEmailAsync(email, subject, body);
    }

    /// <summary>
    /// Envía el correo de bienvenida.
    /// </summary>
    private async Task SendWelcomeEmailAsync(string email, string accessUrl, string? password)
    {
        var subject = "Welcome to The Collabsys - Your Account Details";
        var body = GetWelcomeEmailBody(accessUrl, email, password);
        await SendEmailAsync(email, subject, body);
    }

    /// <summary>
    /// Genera una invitación.
    /// </summary>
    private InvitationDTO GenerateInvitation(string email, bool isExternal)
    {
        return new InvitationDTO
        {
            Email = email,
            Token = Guid.NewGuid(),
            ExpirationDate = DateTime.UtcNow.AddMinutes(5),
            Status = (int)InvitationStatus.Pending,
            IsExternal = isExternal
        };
    }

    /// <summary>
    /// Verifica y actualiza el estado de una invitación.
    /// </summary>
    private async Task<InvitationDTO> VerifyAndUpdateInvitationStatus(Guid token)
    {
        var invitation = await _service.GetByToken(token);

        if (invitation == null)
        {
            throw new KeyNotFoundException("Invitation not found.");
        }

        if (invitation.Status != (int)InvitationStatus.Pending)
        {
            throw new InvalidOperationException("Invitation is already used or expired.");
        }

        if (DateTime.UtcNow > invitation.ExpirationDate)
        {
            invitation.Status = (int)InvitationStatus.Expired;
            await _service.Update(invitation.Id, invitation);
            throw new InvalidOperationException("Invitation has expired.");
        }

        invitation.Status = (int)InvitationStatus.Used;
        await _service.Update(invitation.Id, invitation);

        return invitation;
    }

    /// <summary>
    /// Actualiza o crea un usuario basado en la invitación.
    /// </summary>
    private async Task<(UserDTO? User, bool Success, string? Password)> UpdateOrCreateUser(string email, bool isExternal, int roleId)
    {
        string? password = null;

        var user = await GetUser(email);
        password = isExternal ? PasswordUtils.GeneratePassword(10) : null;
        if (user == null)
        {
            var userCreated = await AddNewUser(email, password);
            if (userCreated == null)
            {
                return (null, false, null);
            }

            var newUserRole = await AddNewUserRole(userCreated.Id, roleId);
            if (newUserRole == null)
            {
                return (null, false, null);
            }

            user = await GetUser(email);
            if (user == null)
            {
                return (null, false, null);
            }

            var company = await GetCompany(email);
            if (company == null)
            {
                return (null, false, null);
            }

            var userCompanyCreated = await AddUserCompany(user.Id, company.CompanyId);
            if (userCompanyCreated == null)
            {
                return (null, false, null);
            }
        }
        else if (isExternal)
        {
            var userToUpdate = new AspNetUser()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.UserName
            };

            await _userService.UpdatePasswordAsync(userToUpdate, password);
        }

        return (user, true, password);
    }


    /// <summary>
    /// Obtiene un usuario por email.
    /// </summary>
    private async Task<UserDTO?> GetUser(string email)
    {
        return await _userService.GetUserByName(email);
    }

    private async Task<CompanyDTO?> GetCompany(string email)
    {
        string domain = email.Split('@')[1];
        return await _companyService.GetByIdDomainAsync(domain);
    }

    /// <summary>
    /// Crea un nuevo usuario.
    /// </summary>
    private async Task<AspNetUser?> AddNewUser(string email, string? password)
    {
        var newUser = new AspNetUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = email,
            Email = email
        };

        var result = password != null
            ? await _userService.AddUserPasswordAsync(newUser, password)
            : await _userService.AddUserAsync(newUser);

        return result;
    }

    /// <summary>
    /// Asigna un rol a un usuario.
    /// </summary>
    private async Task<AspNetUserRole> AddNewUserRole(string userId, int roleId)
    {
        var newUserRole = new AspNetUserRole()
        {
            UserId = userId,
            RoleId = roleId.ToString()
        };

        return await _userRoleService.AddUserRoleAsync(newUserRole);
    }
    private async Task<DdUserCompany> AddUserCompany(string userId, int companyId)
    {
        var userCompany = new DdUserCompany()
        {
            UserId = userId,
            CompanyId = companyId
        };

        return await _userCompanyService.Create(userCompany);
    }

    private async Task UpdateInvitationAsync(DdInvitation existingEntity, InvitationDTO newValues)
    {
        existingEntity.Token = newValues.Token;
        existingEntity.ExpirationDate = newValues.ExpirationDate;
        existingEntity.IsExternal = newValues.IsExternal;
        existingEntity.Status = newValues.Status;
    }
    private async Task UpdateWireListAsync(DdWireList existingEntity, WireListDTO newValues)
    {
        existingEntity.IsExternal = newValues.IsExternal;
        existingEntity.RoleId = newValues.RoleId;
        existingEntity.IsBlackList = newValues.IsBlackList;
        existingEntity.Domain = newValues.Domain;
    }
    #endregion
}