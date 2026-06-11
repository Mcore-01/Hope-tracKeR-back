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
    public async Task<ActionResult<IEnumerable<ItemResponseDto>>> GetAllItems([FromBody] ItemFilterDto filter)
    {
        var items = await _service.GetItemsByFilters(filter);
        return Ok(items);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ItemResponseDto>> GetItemById(int id)
    {
        return Ok(await _service.GetItemById(id));
    }
    [HttpPost("create")]
    public async Task<ActionResult<int>> CreateItem([FromBody] ItemModifyDto item)
    {
        return await _service.CreateItem(item);
    }
    [HttpPut("update")]
    public async Task<ActionResult> UpdateItem([FromBody] ItemModifyDto item)
    {
        return Ok(await _service.UpdateItem(item));
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveItem(int id)
    {
        return Ok(await _service.RemoveItem(id));
    }
}