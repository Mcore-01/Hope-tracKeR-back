using FluentResults;
using FluentValidation;
using Hope_tracKeR_back.Enums;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class IssuanceService : IIssuanceService
{
    private readonly IItemRepository<Device> _repository;
    private readonly IIssuanceRepository _issuanceRepository;
    private readonly IValidator<IssueDeviceRequest> _validator;

    public IssuanceService(
        IItemRepository<Device> repository,
        IIssuanceRepository issuanceRepository,
        IValidator<IssueDeviceRequest> validator)
    {
        _repository = repository;
        _issuanceRepository = issuanceRepository;
        _validator = validator;
    }

    public async Task<Result> IssueDevice(IssueDeviceRequest request)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("\n", validationResult.Errors);
                return Result.Fail(new ValidationError(errors));
            }

            var item = await _repository.GetById(request.ItemId);

            if (item.Status != DeviceStatus.InStock)
                return Result.Fail(new InvalidOperationError("Выдать можно только технику со статусом «В наличии»!"));

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
            await _repository.Update(item);

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
