using Hope_tracKeR_back.Controllers.Interfaces;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hope_tracKeR_back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase, ICatalogController<Category>
    {
        private readonly ICatalogService<Category> _service;
        public CategoryController(ICatalogService<Category> service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<ActionResult<int>> Create([FromBody] Category category)
        {
            var result = await _service.Create(category);

            if (result.IsSuccess)
                return Ok(result.Value);

            if (result.Errors.First() is ValidationError)
                return BadRequest(result.Errors.First().Message);

            if (result.Errors.First() is InvalidOperationError)
                return Conflict(result.Errors.First().Message);

            return BadRequest(result.Errors.First().Message);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAll()
        {
            var result = await _service.GetAll();

            if (result.IsSuccess)
                return Ok(result.Value);

            return StatusCode(500, result.Errors.First().Message);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            var result = await _service.GetById(id);

            if (result.IsSuccess)
                return Ok(result.Value);

            if (result.Errors.First() is NotFoundError)
                return NotFound(result.Errors.First().Message);

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

        [HttpPut("update")]
        public async Task<ActionResult> Update([FromBody] Category category)
        {
            var result = await _service.Update(category);

            if (result.IsSuccess)
                return Ok();

            if (result.Errors.First() is NotFoundError)
                return NotFound(result.Errors.First().Message);

            if (result.Errors.First() is ValidationError)
                return BadRequest(result.Errors.First().Message);

            if (result.Errors.First() is InvalidOperationError)
                return Conflict(result.Errors.First().Message);

            return BadRequest(result.Errors.First().Message);
        }
    }
}
