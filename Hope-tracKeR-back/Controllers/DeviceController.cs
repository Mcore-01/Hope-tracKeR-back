using Hope_tracKeR_back.Controllers.Interfaces;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hope_tracKeR_back.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeviceController : ControllerBase, IItemController<DeviceRequest, ItemFilter, DeviceResponse>
{
    private readonly IItemService<DeviceRequest, ItemFilter, DeviceResponse> _service;

    public DeviceController(IItemService<DeviceRequest, ItemFilter, DeviceResponse> service)
    {
        _service = service;
    }

    [HttpPost("device/filter")]
    public async Task<ActionResult<IEnumerable<DeviceResponse>>> GetByFilters([FromBody] ItemFilter filter)
    {
        var result = await _service.GetByFilters(filter);

        if (result.IsSuccess)
            return Ok(result.Value);

        return StatusCode(500, result.Errors.First().Message);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DeviceResponse>> GetById(int id)
    {
        var result = await _service.GetById(id);

        if (result.IsSuccess)
            return Ok(result.Value);

        if (result.Errors.First() is NotFoundError)
            return NotFound(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpPost("create")]
    public async Task<ActionResult<int>> Create([FromBody] DeviceRequest item)
    {
        var result = await _service.Create(item);

        if (result.IsSuccess)
            return Ok(result.Value);

        if (result.Errors.First() is ValidationError)
            return BadRequest(result.Errors.First().Message);

        if (result.Errors.First() is InvalidOperationError)
            return Conflict(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpPut("update")]
    public async Task<ActionResult> Update([FromBody] DeviceRequest item)
    {
        var result = await _service.Update(item);

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

    [HttpPost("start_repair")]
    public async Task<ActionResult> StartRepairDevice([FromBody] StartRepairRequest repair)
    {
        var result = await _service.StartRepair(repair);

        if (result.IsSuccess)
            return Ok();

        if (result.Errors.First() is NotFoundError)
            return NotFound(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpPost("end_repair")]
    public async Task<ActionResult> CompleteRepair([FromBody] CompleteRepairRequest repair)
    {
        var result = await _service.CompleteRepair(repair);

        if (result.IsSuccess)
            return Ok();

        if (result.Errors.First() is NotFoundError)
            return NotFound(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpPost("excel_items")]
    public async Task<IActionResult> ExportIDevicesToExcel([FromBody] ItemFilter filter)
    {
        var result = await _service.ExportDevicesToExcel(filter);

        if (result.IsSuccess)
            return File(result.Value, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Список_предметов.xlsx"); 

        if (result.Errors.First().Message.Contains("Совпадений по фильтрам не найдено!"))
            return NotFound(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }

    [HttpPost("repair_act/{repairId}")]
    public async Task<IActionResult> GenerateRepairActDocx(int repairId)
    {
        var result = await _service.GenerateRepairActToDocx(repairId);

        if (result.IsSuccess)
            return File(result.Value, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"Акт_приема_передачи.docx");

        if (result.Errors.First().Message.Contains("Отчет о ремонте не найден!"))
            return NotFound(result.Errors.First().Message);

        return BadRequest(result.Errors.First().Message);
    }
}