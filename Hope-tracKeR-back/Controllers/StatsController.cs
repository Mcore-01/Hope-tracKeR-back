using FluentResults;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hope_tracKeR_back.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatsController : ControllerBase
{
    private readonly IStatsService _service;
    public StatsController(IStatsService service)
    {
        _service = service;
    }

    [HttpGet("devices")]
    public async Task<ActionResult<StatsResponse>> GetDevicesStats()
    {
        var result = await _service.GetDevicesStats();

        if (result.IsSuccess)
            return Ok(result.Value);

        return HandleError(result.Errors.First());
    }

    private ActionResult HandleError(IError error)
    {
        return error switch
        {
            NotFoundError => NotFound(error.Message),
            ValidationError => BadRequest(error.Message),
            InvalidOperationError => Conflict(error.Message),
            _ => StatusCode(500, error.Message)
        };
    }
}