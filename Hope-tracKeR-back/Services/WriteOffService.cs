using FluentResults;
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
    private readonly IItemRepository<Device, ItemFilter> _repository;
    public WriteOffService(IItemRepository<Device, ItemFilter> repository, IWriteOffRepository writeOffRepository)
    {   
        _repository = repository;
        _writeOffRepository = writeOffRepository;
    }

    public async Task<Result> WriteOff(int itemId, int userId)
    {
        try
        {
            var item = await _repository.GetById(itemId);
            item.Status = DeviceStatus.WriteOff;

            var writeOff = new WriteOff
            {
                Date = DateTime.UtcNow,
                ItemId = item.Id,
                UserId = userId
            };

            await _writeOffRepository.Create(writeOff);

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