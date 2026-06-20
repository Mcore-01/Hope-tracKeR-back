using FluentResults;
using Hope_tracKeR_back.Controllers.Interfaces;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hope_tracKeR_back.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeviceController : ControllerBase, IItemController<DeviceRequest, DeviceResponse>
{
    private readonly IItemService<DeviceRequest, DeviceResponse> _service;
    private readonly IRepairService _repairService;
    private readonly IWriteOffService _writeOffService;
    private readonly IIssuanceService _issuanceService;
    public DeviceController(IItemService<DeviceRequest, DeviceResponse> service, IRepairService repairService, IWriteOffService writeOffService, IIssuanceService issuanceService)
    {
        _service = service;
        _repairService = repairService;
        _writeOffService = writeOffService;
        _issuanceService = issuanceService;
    }

    [HttpPost("device/filter")]
    public async Task<ActionResult<IEnumerable<DeviceResponse>>> GetByFilters([FromBody] ItemFilter filter)
    {
        var result = await _service.GetByFilters(filter);

        if (result.IsSuccess)
            return Ok(result.Value);

        return HandleError(result.Errors.First());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DeviceResponse>> GetById(int id)
    {
        var result = await _service.GetById(id);

        if (result.IsSuccess)
            return Ok(result.Value);

        return HandleError(result.Errors.First());
    }

    [HttpPost("create")]
    public async Task<ActionResult<int>> Create([FromBody] DeviceRequest item)
    {
        var result = await _service.Create(item);

        if (result.IsSuccess)
            return Ok(result.Value);

        return HandleError(result.Errors.First());
    }

    [HttpPut("update")]
    public async Task<ActionResult> Update([FromBody] DeviceRequest item)
    {
        var result = await _service.Update(item);

        if (result.IsSuccess)
            return Ok();

        return HandleError(result.Errors.First());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Remove(int id)
    {
        var result = await _service.Remove(id);

        if (result.IsSuccess)
            return Ok();

        return HandleError(result.Errors.First());
    }

    [HttpPost("start_repair")]
    public async Task<ActionResult> StartRepairDevice([FromBody] StartRepairRequest repair)
    {
        var result = await _repairService.StartRepair(repair);

        if (result.IsSuccess)
            return Ok();

        return HandleError(result.Errors.First());
    }

    [HttpPost("end_repair")]
    public async Task<ActionResult> CompleteRepairDevice([FromBody] CompleteRepairRequest repair)
    {
        var result = await _repairService.CompleteRepair(repair);

        if (result.IsSuccess)
            return Ok();

        return HandleError(result.Errors.First());
    }

    [HttpPost("excel_items")]
    public async Task<IActionResult> ExportIDevicesToExcel([FromBody] ItemFilter filter)
    {
        var result = await _service.ExportItemsToExcel(filter);

        if (result.IsSuccess)
            return File(result.Value, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Список_предметов.xlsx");

        return HandleError(result.Errors.First());
    }

    [HttpPost("repair_act/{itemID}")]
    public async Task<IActionResult> GenerateRepairActDocx(int itemID)
    {
        var result = await _repairService.GenerateRepairActToDocx(itemID);

        if (result.IsSuccess)
            return File(result.Value, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", $"Акт_приема_передачи.docx");

        if (result.IsSuccess)
            return Ok();

        return HandleError(result.Errors.First());
    }

    [HttpPut("write_off/{itemId}/{userId}")]
    public async Task<ActionResult> WriteOffDevice(int itemId, int userId)
    {
        var result = await _writeOffService.WriteOff(itemId, userId);

        if (result.IsSuccess)
            return Ok();

        return HandleError(result.Errors.First());
    }

    [HttpPost("issue")]
    public async Task<ActionResult> IssueDevice([FromBody] IssueDeviceRequest request)
    {
        var result = await _issuanceService.IssueDevice(request);

        if (result.IsSuccess)
            return Ok();

        return HandleError(result.Errors.First());
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