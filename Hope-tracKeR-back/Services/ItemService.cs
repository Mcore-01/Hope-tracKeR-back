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

    public async Task<IEnumerable<ItemResponseDto>> GetItemsByFilters(ItemFilterDto filter)
    {
        var items = await _itemRepository.GetItemsByFilters(filter);

        return items.Select(MapToResponseDto).ToList();
    }
    public async Task<ItemResponseDto?> GetItemById(int id)
    {
        var response = await _itemRepository.GetItemById(id);
        return MapToResponseDto(response);
    }
    public async Task<int> CreateItem(ItemModifyDto item)
    {
        return await _itemRepository.CreateItem(item);
    }
    public async Task<bool> UpdateItem(ItemModifyDto item)
    {
        return await _itemRepository.UpdateItem(item);
    }
    public async Task<bool> RemoveItem(int id)
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