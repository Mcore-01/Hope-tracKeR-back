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
    public ItemService(IItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<Result<IEnumerable<ItemResponseDto>>> GetItemsByFilters(ItemFilterDto filter)
    {
        var result = await _itemRepository.GetItemsByFilters(filter);

        if (result.IsFailed)
            return Result.Fail<IEnumerable<ItemResponseDto>>(result.Errors);

        return Result.Ok<IEnumerable<ItemResponseDto>>(result.Value.Select(MapToResponseDto));
    }
    public async Task<Result<ItemResponseDto>> GetItemById(int id)
    {
        var result = await _itemRepository.GetItemById(id);

        if (result.IsFailed)
            return Result.Fail<ItemResponseDto>(result.Errors);
        
        return Result.Ok<ItemResponseDto>(MapToResponseDto(result.Value));
    }
    public async Task<Result<int>> CreateItem(ItemModifyDto item)
    {
        return await _itemRepository.CreateItem(item);
    }
    public async Task<Result> UpdateItem(ItemModifyDto item)
    {
        return await _itemRepository.UpdateItem(item);
    }
    public async Task<Result> RemoveItem(int id)
    {
        return await (_itemRepository.RemoveItem(id));  
    }

    public ItemResponseDto MapToResponseDto(Item item)
    {
        return new ItemResponseDto
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
}