﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TheCollabSys.Backend.Entity.DTOs;
using TheCollabSys.Backend.Entity.Response;

namespace TheCollabSys.Backend.API.Controllers;

public abstract class BaseController : ControllerBase
{
    protected IActionResult CreateResponse<T>(string status, T data, string message)
    {
        var response = new Response<T>
        {
            Status = status,
            Data = data,
            Message = message
        };
        return Ok(response);
    }

    protected IActionResult CreateNotFoundResponse<T>(T data, string message)
    {
        var response = new Response<T>
        {
            Status = "99",
            Data = data,
            Message = message
        };

        return NotFound(response);
    }

    protected IActionResult CreateBadRequestResponse<T>(T data, string message)
    {
        var response = new Response<T>
        {
            Status = "99",
            Data = data,
            Message = message
        };

        return BadRequest(response);
    }

    protected async Task<IActionResult> ExecuteAsync(Func<Task<IActionResult>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }

    protected async Task<(string fileType, byte[] fileBytes)> ProcessFileAsync(IFormFile file)
    {
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();
            var fileType = file.ContentType;
            return (fileType, fileBytes);
        }
    }

    protected async Task<IActionResult> HandleClientOperationAsync<T>(string dto, IFormFile? file, Func<T, Task<IActionResult>> clientOperation) where T : class
    {
        try
        {
            var userId = HttpContext.Request.Headers["User-Id"].ToString();
            var model = JsonConvert.DeserializeObject<T>(dto);
            if (model == null)
            {
                return BadRequest("Invalid client data");
            }

            if (file != null && file.Length > 0)
            {
                (string fileType, byte[] fileBytes) = await ProcessFileAsync(file);
                var modelProperties = typeof(T).GetProperties();
                var logoProperty = modelProperties.FirstOrDefault(p => p.Name.Equals("Image", StringComparison.OrdinalIgnoreCase) || p.Name.Equals("Logo", StringComparison.OrdinalIgnoreCase));
                var fileTypeProperty = modelProperties.FirstOrDefault(p => p.Name.Equals("FileType", StringComparison.OrdinalIgnoreCase));
                if (logoProperty != null && fileTypeProperty != null)
                {
                    logoProperty.SetValue(model, fileBytes);
                    fileTypeProperty.SetValue(model, fileType);
                }
            }

            var userIdProperty = typeof(T).GetProperty("UserId");
            if (userIdProperty != null)
            {
                userIdProperty.SetValue(model, userId);
            }

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
}