using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hope_tracKeR_back.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemController : ControllerBase
{
    private readonly IItemService _service;
    public ItemController(IItemService service)
    {
        _service = service;
    }

    [HttpPost("items")]
    public async Task<ActionResult<IEnumerable<ItemResponse>>> GetAllItems([FromBody] ItemFilter filter)
    {
        var result = await _service.GetItemsByFilters(filter);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemResponse>> GetItemById(int id)
    {
        var result = await _service.GetItemById(id);

        if (result.IsSuccess)
            return Ok(result.Value);

        if (result.Errors.First().Message.Contains("Предмет не найден!"))
            return NotFound(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpPost("create")]
    public async Task<ActionResult<int>> CreateItem([FromBody] ItemModify item)
    {
        var result = await _service.CreateItem(item);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpPut("update")]
    public async Task<ActionResult> UpdateItem([FromBody] ItemModify item)
    {
        var result = await _service.UpdateItem(item);

        if (result.IsSuccess)
            return Ok();

        if (result.Errors.First().Message.Contains("Предмет не найден!"))
            return NotFound(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveItem(int id)
    {
        var result = await _service.RemoveItem(id);

        if (result.IsSuccess)
            return Ok();

        if (result.Errors.First().Message.Contains("Предмет не найден!"))
            return NotFound(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpPost("start_repair")]
    public async Task<ActionResult> StartRepairItem([FromBody] StartRepairRequest repair)
    {
        var result = await _service.StartRepairItem(repair);

        if (result.IsSuccess)
            return Ok();

        if (result.Errors.First().Message.Contains("Предмет не найден!"))
            return NotFound(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpPost("end_repair")]
    public async Task<ActionResult> CompleteRepairItem([FromBody] CompleteRepairRequest repair)
    {
        var result = await _service.CompleteRepairItem(repair);

        if (result.IsSuccess)
            return Ok();

        if (result.Errors.First().Message.Contains("Предмет не найден!"))
            return NotFound(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }
}