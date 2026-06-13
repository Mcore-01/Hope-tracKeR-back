using ClosedXML.Excel;
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

    public async Task<Result<byte[]>> ExportItemsToExcel(ItemFilter filter)
    {
        var result = await _itemRepository.GetItemsByFilters(filter);

        if (result.IsFailed)
            return Result.Fail<byte[]>(result.Errors);

        var items = result.Value.Select(MapToResponseDto).ToList();

        if (items is null || items.Count == 0)
        {
            return Result.Fail<byte[]>("Совпадений по фильтрам не найдено!");
        }

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Brands");

            worksheet.Cell(1, 1).Value = "Индентификатор в базе";
            worksheet.Cell(1, 2).Value = "Наименование";
            worksheet.Cell(1, 3).Value = "Бренд";
            worksheet.Cell(1, 4).Value = "Серийный номер";
            worksheet.Cell(1, 5).Value = "Категория";
            worksheet.Cell(1, 6).Value = "Статус";
            worksheet.Cell(1, 7).Value = "Добавлен в базу";
            worksheet.Cell(1, 8).Value = "Адрес хранения";

            var headerRow = worksheet.Row(1);
            headerRow.Style.Font.Bold = true;
            headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
            headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


            for (int i = 0; i < items.Count; i++)
            {
                worksheet.Cell(i + 2, 1).Value = items[i].Id;
                worksheet.Cell(i + 2, 2).Value = items[i].Name;
                worksheet.Cell(i + 2, 3).Value = items[i].Brand;
                worksheet.Cell(i + 2, 4).Value = items[i].SerialId;
                worksheet.Cell(i + 2, 5).Value = items[i].Category;
                worksheet.Cell(i + 2, 6).Value = items[i].Status;
                worksheet.Cell(i + 2, 7).Value = items[i].AddedDate;
                worksheet.Cell(i + 2, 8).Value = items[i].Address;
            }

            worksheet.Columns().AdjustToContents();

            using (var stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                return Result.Ok<byte[]>(stream.ToArray());
            }
        }
    }
}