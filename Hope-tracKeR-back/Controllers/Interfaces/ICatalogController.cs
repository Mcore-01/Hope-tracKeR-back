using Microsoft.AspNetCore.Mvc;

namespace Hope_tracKeR_back.Controllers.Interfaces;

public interface ICatalogController<T>
{
    Task<ActionResult<IEnumerable<T>>> GetAll();
    Task<ActionResult<T>> GetById(int id);
    Task<ActionResult<int>> Create([FromBody] T value);
    Task<ActionResult> Update([FromBody] T value);
    Task<ActionResult> Remove(int id);
}