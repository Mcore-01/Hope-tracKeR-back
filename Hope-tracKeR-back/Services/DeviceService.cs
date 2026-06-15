using AutoMapper;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2016.Excel;
using FluentResults;
using FluentValidation;
using Hope_tracKeR_back.Enums;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Hope_tracKeR_back.Services;

public class DeviceService : IItemService
{
    private readonly IItemRepository<Device, ItemFilter> _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<DeviceModify> _validator;
    private readonly IRepairRepository _repairRepository;
    public DeviceService(IItemRepository<Device, ItemFilter> itemRepository, IMapper mapper, IValidator<DeviceModify> validator, IRepairRepository repairRepository)
    {
        _repository = itemRepository;
        _mapper = mapper;
        _validator = validator;
        _repairRepository = repairRepository;
    }

    public async Task<Result<IEnumerable<DeviceResponse>>> GetByFilters(ItemFilter filter)
    {
        try
        {
            var items = await _repository.GetByFilters(filter);

            return Result.Ok(items.Select(MapToResponseDto));
        }
        catch (Exception ex)
        {
            return Result.Fail<IEnumerable<DeviceResponse>>(new Error($"Произошла ошибка: {ex.Message}"));
        }        
    }

    public async Task<Result<DeviceResponse>> GetById(int id)
    {
        try
        {
            var item = await _repository.GetById(id);

            if (item is null)
                return Result.Fail<DeviceResponse>(new NotFoundError(nameof(Device), id));

            return Result.Ok(MapToResponseDto(item));
        }
        catch (Exception ex)
        {
            return Result.Fail<DeviceResponse>(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public async Task<Result<int>> Create(DeviceModify itemModify)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(itemModify);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("\n", validationResult.Errors);
                return Result.Fail<int>(new ValidationError(errors));
            }

            var item = _mapper.Map<Device>(itemModify);

            var itemId = await _repository.Create(item);

            return Result.Ok(itemId);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Fail<int>(new InvalidOperationError(ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Fail<int>(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public async Task<Result> Update(DeviceModify itemModify)
    {
        try
        {
            var validationResult = await _validator.ValidateAsync(itemModify);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("\n", validationResult.Errors);
                return Result.Fail(new ValidationError(errors));
            }

            var item = _mapper.Map<Device>(itemModify);

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

    public async Task<Result> Remove(int id)
    {
        try
        {
            await _repository.Remove(id);

            return Result.Ok();
        }
        catch (NullReferenceException ex)
        {
            return Result.Fail(new NotFoundError(ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public DeviceResponse MapToResponseDto(Device item)
    {
        return new DeviceResponse
        {
            Id = item.Id,
            Name = item.Name,
            SerialId = item.SerialNumber,
            Status = item.Status.GetDisplayName(),
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

    public async Task<Result> CompleteRepair(CompleteRepairRequest repairRequest)
    {
        var result = await _repairRepository.CompleteRepair(repairRequest);

        return result;
    }

    public async Task<Result<byte[]>> ExportDevicesToExcel(ItemFilter filter)
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
        }
        catch (Exception)
        {
            return Result.Fail<byte[]>(new Error("Произошла ошибка при генерации документа!"));
        }
    }

    public async Task<Result<byte[]>> GenerateRepairActToDocx(int repairId)
    {
        try
        {
            var result = await _repairRepository.GetRepairById(repairId);

            if (result.IsFailed)
                return Result.Fail<byte[]>(result.Errors);

            var repair = result.Value;

            using var stream = new MemoryStream();

            using (var doc = DocX.Create(stream))
            {
                doc.InsertParagraph("АКТ ПРИЕМА-ПЕРЕДАЧИ").FontSize(16).Bold().Alignment = Alignment.center;

                doc.InsertParagraph("техники в ремонт").FontSize(14).Bold().Alignment = Alignment.center;

                doc.InsertParagraph($"№ {repair.Id}").FontSize(14).Bold().Alignment = Alignment.center;
                doc.InsertParagraph();

                doc.InsertParagraph($"«{repair.StartDate:dd}» {repair.StartDate.Month.ToString().PadLeft(2, '0')} {repair.StartDate:yyyy} г.");
                doc.InsertParagraph();

                doc.InsertParagraph($"Текущее местоположение: {repair.Address.Branch}, {repair.Address.Building}, {repair.Address.Floor}, {repair.Address.Room}");
                doc.InsertParagraph();

                doc.InsertParagraph("Оборудование, передаваемое в ремонт:").Bold();

                var table = doc.AddTable(2, 4);

                table.Rows[0].Cells[0].Paragraphs.First().Append("Идентификатор отчета").Bold();
                table.Rows[0].Cells[1].Paragraphs.First().Append("Наименование").Bold();
                table.Rows[0].Cells[2].Paragraphs.First().Append("Серийный номер").Bold();
                table.Rows[0].Cells[3].Paragraphs.First().Append("Неисправность").Bold();

                table.Rows[1].Cells[0].Paragraphs.First().Append(repair.ItemId.ToString());
                table.Rows[1].Cells[1].Paragraphs.First().Append(repair.Item.Name);
                table.Rows[1].Cells[2].Paragraphs.First().Append(repair.Item.SerialNumber);
                table.Rows[1].Cells[3].Paragraphs.First().Append(repair.Description);

                doc.InsertTable(table);
                doc.InsertParagraph();

                if (!string.IsNullOrEmpty(repair.Diagnosis))
                {
                    doc.InsertParagraph("Результаты диагностики:").Bold();
                    doc.InsertParagraph(repair.Diagnosis);
                    doc.InsertParagraph();
                }

                doc.InsertParagraph($"Статус ремонта: {repair.Status.GetDisplayName()}").Bold();
                doc.InsertParagraph();

                doc.Save();

                return Result.Ok(stream.ToArray());
            }
        }
        catch (Exception)
        {
            return Result.Fail<byte[]>(new Error("Произошла ошибка при генерации документа!"));
        }
    }
}