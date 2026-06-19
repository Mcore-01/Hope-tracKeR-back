using AutoMapper;
using ClosedXML.Excel;
using FluentResults;
using FluentValidation;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;

namespace Hope_tracKeR_back.Services;

public class ConsumableService : BaseItemService<Consumable, ConsumableRequest, ConsumableResponse>
{
    public ConsumableService(IItemRepository<Consumable> repository, IMapper mapper, IValidator<ConsumableRequest> validator)
        : base(repository, mapper, validator) { }

    public override async Task<Result<byte[]>> ExportItemsToExcel(ItemFilter filter)
    {
        try
        {
            var result = await _repository.GetByFilters(filter);


            var items = result.ToList();

            if (items is null || items.Count == 0)
            {
                return Result.Fail<byte[]>("Совпадений по фильтрам не найдено!");
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Расходники");

                worksheet.Cell(1, 1).Value = "Идентификатор в базе";
                worksheet.Cell(1, 2).Value = "Наименование";
                worksheet.Cell(1, 3).Value = "Бренд";
                worksheet.Cell(1, 4).Value = "Адрес хранения";
                worksheet.Cell(1, 5).Value = "Количество";

                var headerRow = worksheet.Row(1);
                headerRow.Style.Font.Bold = true;
                headerRow.Style.Fill.BackgroundColor = XLColor.LightGray;
                headerRow.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


                for (int i = 0; i < items.Count; i++)
                {
                    worksheet.Cell(i + 2, 1).Value = items[i].Id;
                    worksheet.Cell(i + 2, 2).Value = items[i].Name;
                    worksheet.Cell(i + 2, 3).Value = items[i].Brand.Name;
                    worksheet.Cell(i + 2, 4).Value = $"{items[i].Address.Branch}, {items[i].Address.Building}, {items[i].Address.Floor}, {items[i].Address.Room}";
                    worksheet.Cell(i + 2, 5).Value = items[i].Quantity;
                }

                worksheet.Columns().AdjustToContents();

                using var stream = new MemoryStream();
                workbook.SaveAs(stream);

                return Result.Ok<byte[]>(stream.ToArray());
            }
        }
        catch (Exception)
        {
            return Result.Fail<byte[]>(new Error("Произошла ошибка при генерации документа!"));
        }
    }
}