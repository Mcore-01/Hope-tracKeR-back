using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Hope_tracKeR_back.Controllers.Interfaces;

public interface IItemController<TRequest, TResponse> where TResponse : class
{
    Task<ActionResult<PagedListResponse<TResponse>>> GetByFilters([FromBody] ItemFilterRequest filter);
    Task<ActionResult<TResponse>> GetById(int id);
    Task<ActionResult<int>> Create([FromBody] TRequest request);
    Task<ActionResult> Update([FromBody] TRequest request);
    Task<ActionResult> Remove(int id);
}