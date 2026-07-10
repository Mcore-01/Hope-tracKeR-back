using FluentResults;
using FluentValidation;
using Hope_tracKeR_back.Constants;
using Hope_tracKeR_back.Enums;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class IssuanceService : IIssuanceService
{
    private readonly IIssuanceRepository _issuanceRepository;
    private readonly IValidator<IssueDeviceRequest> _validator;
    private readonly IAuditLogService _auditLog;

    public IssuanceService(IIssuanceRepository issuanceRepository, IValidator<IssueDeviceRequest> validator, IAuditLogService auditLog)
    {
        _issuanceRepository = issuanceRepository;
        _validator = validator;
        _auditLog = auditLog;
    }

    public async Task<Result> IssueDevice(IssueDeviceRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("\n", validationResult.Errors);
            return Result.Fail(new ValidationError(errors));
        }

        try
        {
            var item = await _issuanceRepository.GetDeviceById(request.ItemId);

            if (item.Status != DeviceStatus.InStock)
                return Result.Fail(new InvalidOperationError("Выдать можно только технику со статусом «В наличии»!"));

            var oldDevice = new { item.Id, item.Status, item.EmployeeId };

            item.Status = DeviceStatus.Issued;
            item.EmployeeId = request.EmployeeId;

            var issuance = new Issuance
            {
                Date = DateTime.UtcNow,
                ItemId = item.Id,
                EmployeeId = request.EmployeeId,
                UserId = request.UserId
            };

            await _issuanceRepository.Create(issuance);
            await _issuanceRepository.UpdateDevice(item);
            await _issuanceRepository.SaveChangesAsync();

            await _auditLog.LogAsync(AuditActions.Issue, nameof(Issuance), issuance.Id.ToString(), issuance);
            await _auditLog.LogAsync(AuditActions.Update, nameof(Device), item.Id.ToString(), new { item.Id, item.Status, item.EmployeeId });

            return Result.Ok();
        }
        catch (NullReferenceException ex)
        {
            return Result.Fail(new NotFoundError(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return Result.Fail(new InvalidOperationError(ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }
}