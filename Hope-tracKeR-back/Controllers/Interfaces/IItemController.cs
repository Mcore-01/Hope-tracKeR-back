using Microsoft.AspNetCore.Mvc;

namespace Hope_tracKeR_back.Controllers.Interfaces;

public interface IItemController<TRequest, TFilter, TResponse>
{
    Task<ActionResult<IEnumerable<TResponse>>> GetByFilters([FromBody] TFilter filter);
    Task<ActionResult<TResponse>> GetById(int id);
    Task<ActionResult<int>> Create([FromBody] TRequest request);
    Task<ActionResult> Update([FromBody] TRequest request);
    Task<ActionResult> Remove(int id);
}