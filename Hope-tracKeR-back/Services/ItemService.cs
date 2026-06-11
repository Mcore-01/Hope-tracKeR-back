using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;
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

        return items.Select(i => new ItemResponseDto
        {
            Id = i.Id,
            Name = i.Name,
            SerialId = i.SerialId,
            Category = i.Category.ToString(),
            Status = i.Status.ToString(),
            AddedDate = i.AddedDate,
            AddressId = i.AddressId,
            Address = $"{i.Address.Branch}, {i.Address.Building}, {i.Address.Floor}, {i.Address.Room}",
            BrandId = i.BrandId,
            Brand = i.Brand.Name,
            Attributes = i.Attributes.ToDictionary(a => a.Name, a => a.Value)
        });
    }
}