using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;
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
    public async Task<ActionResult<IEnumerable<Brand>>> GetAllBrands([FromBody] ItemFilterDto filter)
    {
        var items = await _service.GetItemsByFilters(filter);
        return Ok(items);
    }
}