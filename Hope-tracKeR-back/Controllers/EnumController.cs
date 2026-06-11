using Hope_tracKeR_back.Enums;
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

    [HttpGet("brands")]
    public async Task<ActionResult<IEnumerable<Brand>>> GetAllBrands()
    {
        var brands = await _service.GetAllBrands();
        return Ok(brands);
    }

    [HttpGet("addresses")]
    public async Task<ActionResult<IEnumerable<Address>>> GetAllAddresses()
    {
        var addresses = await _service.GetAllAddresses();
        return Ok(addresses);
    }
}