using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;
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

    [HttpGet("addresses")]
    public async Task<ActionResult<IEnumerable<Address>>> GetAllAddresses()
    {
        var result = await _service.GetAllAddresses();

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IEnumerable<Brand>>> GetAllBrands()
    {
        var result = await _service.GetAllBrands();

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BrandDto>> GetBrandById(int id)
    {
        var result = await _service.GetBrandById(id);

        if (result.IsSuccess)
            return Ok(result.Value);

        if (result.Errors.First().Message.Contains("Бренд не найден!"))
            return NotFound(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpPost("create")]
    public async Task<ActionResult<int>> CreateBrand([FromBody] BrandDto brand)
    {
        var result = await _service.CreateBrand(brand);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpPut("update")]
    public async Task<ActionResult> UpdateBrand([FromBody] BrandDto brand)
    {
        var result = await _service.UpdateBrand(brand);

        if (result.IsSuccess)
            return Ok();

        if (result.Errors.First().Message.Contains("Бренд не найден!"))
            return NotFound(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> RemoveBrand(int id)
    {
        var result = await _service.RemoveBrand(id);

        if (result.IsSuccess)
            return Ok();

        if (result.Errors.First().Message.Contains("Бренд не найден!"))
            return NotFound(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }
}