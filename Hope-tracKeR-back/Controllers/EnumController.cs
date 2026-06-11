using Hope_tracKeR_back.Models;
using Hope_tracKeR_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hope_tracKeR_back.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EnumController : ControllerBase
{
    private readonly IEnumService _service;

    public EnumController(IEnumService service)
    {
        _service = service;
    }
    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
    {
        var categories = await _service.GetAllCategories();
        return Ok(categories);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IEnumerable<Brand>>> GetAllBrands()
    {
        var brands = await _service.GetAllBrands();
        return Ok(brands);
    }
}