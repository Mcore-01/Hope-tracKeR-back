using FluentResults;
using Hope_tracKeR_back.Controllers.Interfaces;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hope_tracKeR_back.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartridgeController : ControllerBase, IItemController<CartridgeRequest, CartridgeResponse>
{
    private readonly IItemService<CartridgeRequest, CartridgeResponse> _service;
    public CartridgeController(IItemService<CartridgeRequest, CartridgeResponse> service)
    {
        _service = service;
    }

    [HttpPost("Cartridges/filter")]
    public async Task<ActionResult<IEnumerable<CartridgeResponse>>> GetByFilters([FromBody] ItemFilter filter)
    {
        var result = await _service.GetByFilters(filter);

        if (result.IsSuccess)
            return Ok(result.Value);

        return HandleError(result.Errors.First());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CartridgeResponse>> GetById(int id)
    {
        var result = await _service.GetById(id);

        if (result.IsSuccess)
            return Ok(result.Value);

        return HandleError(result.Errors.First());
    }

    [HttpPost("create")]
    public async Task<ActionResult<int>> Create([FromBody] CartridgeRequest item)
    {
        var result = await _service.Create(item);

        if (result.IsSuccess)
            return Ok(result.Value);

        return HandleError(result.Errors.First());
    }

    [HttpPut("update")]
    public async Task<ActionResult> Update([FromBody] CartridgeRequest item)
    {
        var result = await _service.Update(item);

        if (result.IsSuccess)
            return Ok();

        return HandleError(result.Errors.First());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Remove(int id)
    {
        var result = await _service.Remove(id);

        if (result.IsSuccess)
            return Ok();

        return HandleError(result.Errors.First());
    }

    [HttpPost("excel_items")]
    public async Task<IActionResult> ExportIDevicesToExcel([FromBody] ItemFilter filter)
    {
        var result = await _service.ExportItemsToExcel(filter);

        if (result.IsSuccess)
            return File(result.Value, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Список_картриджей.xlsx");

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