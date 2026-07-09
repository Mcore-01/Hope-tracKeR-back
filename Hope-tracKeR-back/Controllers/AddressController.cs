using FluentResults;
using Hope_tracKeR_back.Controllers.Interfaces;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.Entities;
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
        if (!result.IsSuccess)
            return HandleError(result.Errors.First());
        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Address>> GetById(int id)
    {
        var result = await _service.GetById(id);
        if (!result.IsSuccess)
            return HandleError(result.Errors.First());
        return Ok(result.Value);
    }

    [HttpPost("create")]
    public async Task<ActionResult<int>> Create([FromBody] Address address)
    {
        var result = await _service.Create(address);
        if (!result.IsSuccess)
            return HandleError(result.Errors.First());
        return Ok(result.Value);
    }

    [HttpPut("update")]
    public async Task<ActionResult> Update([FromBody] Address address)
    {
        var result = await _service.Update(address);
        if (!result.IsSuccess)
            return HandleError(result.Errors.First());
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Remove(int id)
    {
        var result = await _service.Remove(id);
        if (!result.IsSuccess)
            return HandleError(result.Errors.First());
        return Ok();
    }

    private ActionResult HandleError(IError error)
    {
        return error switch
        {
            NotFoundError => NotFound(error.Message),
            ValidationError => BadRequest(error.Message),
            InvalidOperationError => Conflict(error.Message),
            _ => StatusCode(500, error.Message)
        };
    }
}