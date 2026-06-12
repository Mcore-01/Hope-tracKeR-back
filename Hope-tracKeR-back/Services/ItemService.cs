using FluentResults;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class ItemService : IItemService
{
    private readonly IItemRepository _itemRepository;
    private readonly IRepairRepository _repairRepository;
    public ItemService(IItemRepository itemRepository, IRepairRepository repairRepository)
    {
        _itemRepository = itemRepository;
        _repairRepository = repairRepository;
    }

    public async Task<Result<IEnumerable<ItemResponse>>> GetItemsByFilters(ItemFilter filter)
    {
        var result = await _itemRepository.GetItemsByFilters(filter);

        if (result.IsFailed)
            return Result.Fail<IEnumerable<ItemResponse>>(result.Errors);

        return Result.Ok(result.Value.Select(MapToResponseDto));
    }

    public async Task<Result<ItemResponse>> GetItemById(int id)
    {
        var result = await _itemRepository.GetItemById(id);

        if (result.IsFailed)
            return Result.Fail<ItemResponse>(result.Errors);
        
        return Result.Ok(MapToResponseDto(result.Value));
    }

    public async Task<Result<int>> CreateItem(ItemModify item)
    {
        return await _itemRepository.CreateItem(item);
    }

    public async Task<Result> UpdateItem(ItemModify item)
    {
        return await _itemRepository.UpdateItem(item);
    }

    public async Task<Result> RemoveItem(int id)
    {
        return await (_itemRepository.RemoveItem(id));  
    }

    public ItemResponse MapToResponseDto(Item item)
    {
        return new ItemResponse
        {
            Id = item.Id,
            Name = item.Name,
            SerialId = item.SerialId,
            Category = item.Category.ToString(),
            Status = item.Status.ToString(),
            AddedDate = item.AddedDate,
            AddressId = item.AddressId,
            Address = $"{item.Address.Branch}, {item.Address.Building}, {item.Address.Floor}, {item.Address.Room}",
            BrandId = item.BrandId,
            Brand = item.Brand.Name,
            Attributes = item.Attributes.ToDictionary(a => a.Name, a => a.Value)
        };
    }

    public async Task<Result> StartRepairItem(StartRepairRequest repairRequest)
    {
        var result = await _repairRepository.CreateRepair(repairRequest);

        return result;
    }

    public async Task<Result> CompleteRepairItem(CompleteRepairRequest repairRequest)
    {
        var result = await _repairRepository.CompleteRepair(repairRequest);

        return result;
    }
}