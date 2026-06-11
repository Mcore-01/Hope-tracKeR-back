using Hope_tracKeR_back.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hope_tracKeR_back.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EnumController : ControllerBase
{
    [HttpGet("categories")]
    public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
    {
        var categories = new List<Category>();  
        return Ok(categories);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IEnumerable<Brand>>> GetAllBrands()
    {
        var brands = new List<Brand>();
        return Ok(brands);
    }
}