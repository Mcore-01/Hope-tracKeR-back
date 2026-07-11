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

public class WriteOffService : IWriteOffService
{
    private readonly IWriteOffRepository _writeOffRepository;
    private readonly IValidator<WriteOffDeviceRequest> _validator;
    private readonly IAuditLogService _auditLog;

    public WriteOffService(IWriteOffRepository writeOffRepository, IValidator<WriteOffDeviceRequest> validator, IAuditLogService auditLog)
    {
        _writeOffRepository = writeOffRepository;
        _validator = validator;
        _auditLog = auditLog;
    }

    public async Task<Result> WriteOff(WriteOffDeviceRequest writeOffDeviceRequest)
    {
        var validationResult = await _validator.ValidateAsync(writeOffDeviceRequest);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("\n", validationResult.Errors);
            return Result.Fail(new ValidationError(errors));
        }

        try
        {
            var item = await _writeOffRepository.GetDeviceById(writeOffDeviceRequest.ItemId);
            var oldDevice = new { item.Id, item.Status };

            item.Status = DeviceStatus.WriteOff;

            var writeOff = new WriteOff
            {
                Date = DateTime.UtcNow,
                ItemId = item.Id,
                UserId = writeOffDeviceRequest.UserId,
            };

            await _writeOffRepository.Create(writeOff);
            await _writeOffRepository.UpdateDevice(item);
            await _writeOffRepository.SaveChangesAsync();

            await _auditLog.LogAsync(AuditActions.WriteOff, nameof(WriteOff), writeOff.Id.ToString(), writeOff);
            await _auditLog.LogAsync(AuditActions.Update, nameof(Device), item.Id.ToString(), new { item.Id, item.Status });

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