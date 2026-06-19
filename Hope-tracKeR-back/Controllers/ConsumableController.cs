using FluentResults;
using Hope_tracKeR_back.Controllers.Interfaces;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Services;
using Hope_tracKeR_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hope_tracKeR_back.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConsumableController : ControllerBase, IItemController<ConsumableRequest, ConsumableResponse>
{
    private readonly IItemService<ConsumableRequest, ConsumableResponse> _service;
    private readonly IInventoryService _inventoryService;
    public ConsumableController(IItemService<ConsumableRequest, ConsumableResponse> service, IInventoryService inventoryService)
    {
        _service = service;
        _inventoryService = inventoryService;
    }

    [HttpPost("Consumables/filter")]
    public async Task<ActionResult<IEnumerable<ConsumableResponse>>> GetByFilters([FromBody] ItemFilter filter)
    {
        var result = await _service.GetByFilters(filter);

        if (result.IsSuccess)
            return Ok(result.Value);

        return HandleError(result.Errors.First());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ConsumableResponse>> GetById(int id)
    {
        var result = await _service.GetById(id);

        if (result.IsSuccess)
            return Ok(result.Value);

        return HandleError(result.Errors.First());
    }

    [HttpPost("create")]
    public async Task<ActionResult<int>> Create([FromBody] ConsumableRequest item)
    {
        var result = await _service.Create(item);

        if (result.IsSuccess)
            return Ok(result.Value);

        return HandleError(result.Errors.First());
    }

    [HttpPut("update")]
    public async Task<ActionResult> Update([FromBody] ConsumableRequest item)
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
            return File(result.Value, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Список_предметов.xlsx");

        return HandleError(result.Errors.First());
    }

    [HttpPut("increase/{id}/{amount}")]
    public async Task<ActionResult> IncreaseQuantity(int id, int amount)
    {
        var result = await _inventoryService.IncreaseQuantity(id, amount);

        if (result.IsSuccess)
            return Ok();

        return HandleError(result.Errors.First());
    }

    [HttpPut("decrease/{id}/{amount}")]
    public async Task<ActionResult> DecreaseQuantity(int id, int amount)
    {
        var result = await _inventoryService.DecreaseQuantity(id, amount);

        if (result.IsSuccess)
            return Ok();

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