using Hope_tracKeR_back.Controllers.Interfaces;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hope_tracKeR_back.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrandController : ControllerBase, ICatalogController<Brand>
{
    private readonly ICatalogService<Brand> _service;

    public BrandController(ICatalogService<Brand> service)
    {
        _service = service;
    }


    [HttpGet]
    public async Task<ActionResult<IEnumerable<Brand>>> GetAll()
    {
        var result = await _service.GetAll();

        if (result.IsSuccess)
            return Ok(result.Value);

        return StatusCode(500, result.Errors.First().Message);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Brand>> GetById(int id)
    {
        var result = await _service.GetById(id);

        if (result.IsSuccess)
            return Ok(result.Value);

        if (result.Errors.First() is NotFoundError)
            return NotFound(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpPost("create")]
    public async Task<ActionResult<int>> Create([FromBody] Brand brand)
    {
        var result = await _service.Create(brand);

        if (result.IsSuccess)
            return Ok(result.Value);

        if (result.Errors.First() is InvalidOperationError)
            return Conflict(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpPut("update")]
    public async Task<ActionResult> Update([FromBody] Brand brand)
    {
        var result = await _service.Update(brand);

        if (result.IsSuccess)
            return Ok();

        if (result.Errors.First() is NotFoundError)
            return NotFound(result.Errors.First().Message);

        if (result.Errors.First() is InvalidOperationError)
            return Conflict(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Remove(int id)
    {
        var result = await _service.Remove(id);

        if (result.IsSuccess)
            return Ok();

        if (result.Errors.First() is NotFoundError)
            return NotFound(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }
}