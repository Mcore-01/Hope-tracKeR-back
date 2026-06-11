using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Services;
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
    public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetAllItems([FromBody] ItemFilterDto filter)
    {
        var result = await _service.GetItemsByFilters(filter);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Errors.First().Message);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ItemResponseDto>> GetItemById(int id)
    {
        var result = await _service.GetItemById(id);

        if (result.IsSuccess)
            return Ok(result.Value);

        if (result.Errors.First().Message.Contains("Предмет не найден!"))
            return NotFound(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }
    [HttpPost("create")]
    public async Task<ActionResult<int>> CreateItem([FromBody] ItemModifyDto item)
    {
        var result = await _service.CreateItem(item);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Errors.First().Message);
    }
    [HttpPut("update")]
    public async Task<ActionResult> UpdateItem([FromBody] ItemModifyDto item)
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
}