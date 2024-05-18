using Microsoft.AspNetCore.Mvc;
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
}
