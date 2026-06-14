using Hope_tracKeR_back.Controllers.Interfaces;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hope_tracKeR_back.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AddressController : ControllerBase, ICatalogController<Address>
{
    private readonly ICatalogService<Address> _service;
    public AddressController(ICatalogService<Address> service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Address>>> GetAll()
    {
        var result = await _service.GetAll();

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Address>> GetById(int id)
    {
        var result = await _service.GetById(id);

        if (result.IsSuccess)
            return Ok(result.Value);

        if (result.Errors.First().Message.Contains("Бренд не найден!"))
            return NotFound(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpPost("create")]
    public Task<ActionResult<int>> Create([FromBody] Address value)
    {
        throw new NotImplementedException();
    }

    [HttpPut("update")]
    public Task<ActionResult> Update([FromBody] Address value)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id}")]
    public Task<ActionResult> Remove(int id)
    {
        throw new NotImplementedException();
    }
}