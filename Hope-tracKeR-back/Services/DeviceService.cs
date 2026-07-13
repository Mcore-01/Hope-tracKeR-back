using AutoMapper;
using ClosedXML.Excel;
using FluentResults;
using FluentValidation;
using Hope_tracKeR_back.Enums;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class DeviceService : BaseItemService<Device, DeviceRequest, DeviceResponse>
{
    public DeviceService(IItemRepository<Device> repository, IMapper mapper, IValidator<DeviceRequest> validator, IAuditLogService auditLog)
        : base(repository, mapper, validator, auditLog) {}

    public override async Task<Result<byte[]>> ExportItemsToExcel(ItemFilterRequest filter)
    {
        try
        {
            var result = await _repository.GetByFilters(filter);

            var items = result.ToList();

            if (items is null || items.Count == 0)
            {
                return Result.Fail<byte[]>("Совпадений по фильтрам не найдено!");
            }

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Техника");

            worksheet.Cell(1, 1).Value = "Идентификатор в базе";
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
                worksheet.Cell(i + 2, 3).Value = items[i].Brand.Name;
                worksheet.Cell(i + 2, 4).Value = items[i].SerialNumber;
                worksheet.Cell(i + 2, 6).Value = items[i].Status.GetDisplayName();
                worksheet.Cell(i + 2, 7).Value = items[i].AddedDate;
                worksheet.Cell(i + 2, 8).Value = $"{items[i].Address.Branch}, {items[i].Address.Building}, {items[i].Address.Floor}, {items[i].Address.Room}";
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return Result.Ok<byte[]>(stream.ToArray());
        }
        catch (Exception)
        {
            return Result.Fail<byte[]>(new Error("Произошла ошибка при генерации документа!"));
        }
    }
}