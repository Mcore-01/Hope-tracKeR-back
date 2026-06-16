using AutoMapper;
using ClosedXML.Excel;
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

public class DeviceService : IItemService<DeviceRequest, ItemFilter, DeviceResponse>
{
    private readonly IItemRepository<Device, ItemFilter> _repository;
    private readonly IRepairRepository _repairRepository;
    private readonly IWriteOffRepository _writeOffRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<DeviceRequest> _validator;
    private readonly IValidator<StartRepairRequest> _startRepairValidator;
    private readonly IValidator<CompleteRepairRequest> _completeRepairValidator;
    public DeviceService(IItemRepository<Device, ItemFilter> itemRepository, 
        IMapper mapper, IValidator<DeviceRequest> validator, IValidator<StartRepairRequest> startRepairValidator,
        IValidator<CompleteRepairRequest> completeRepairValidator, IRepairRepository repairRepository, IWriteOffRepository writeOffRepository)
    {
        _repository = itemRepository;
        _mapper = mapper;
        _validator = validator;
        _startRepairValidator = startRepairValidator;   
        _completeRepairValidator = completeRepairValidator;
        _repairRepository = repairRepository;
        _writeOffRepository = writeOffRepository;
    }

    public async Task<Result<IEnumerable<DeviceResponse>>> GetByFilters(ItemFilter filter)
    {
        try
        {
            var items = await _repository.GetByFilters(filter);

            var itemsResponse = _mapper.Map<IEnumerable<DeviceResponse>>(items);

            return Result.Ok(itemsResponse);
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

            var itemResponse = _mapper.Map<DeviceResponse>(item);

            return Result.Ok(itemResponse);
        }
        catch (Exception ex)
        {
            return Result.Fail<DeviceResponse>(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public async Task<Result<int>> Create(DeviceRequest itemModify)
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

    public async Task<Result> Update(DeviceRequest itemModify)
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

    public async Task<Result<int>> StartRepair(StartRepairRequest repairRequest)
    {
        try
        {
            var validationResult = await _startRepairValidator.ValidateAsync(repairRequest);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("\n", validationResult.Errors);
                return Result.Fail<int>(new ValidationError(errors));
            }

            var repair = _mapper.Map<Repair>(repairRequest);
            repair.Status = RepairStatus.InProgress;

            var item = await _repository.GetById(repair.ItemId);
            item.Status = DeviceStatus.Repair;
            item.AddressId = repair.AddressId;

            await _repository.Update(item);

            var itemId = await _repairRepository.Create(repair);

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

    public async Task<Result> CompleteRepair(CompleteRepairRequest repairRequest)
    {
        try
        {
            var validationResult = await _completeRepairValidator.ValidateAsync(repairRequest);
            if (!validationResult.IsValid)
            {
                var errors = string.Join("\n", validationResult.Errors);
                return Result.Fail(new ValidationError(errors));
            }

            var item = await _repository.GetById(repairRequest.UserId);
            var repair = await _repairRepository.GetRepairByItemId(repairRequest.UserId);

            repair.Status = RepairStatus.Completed;
            repair.EndDate = repairRequest.EndDate;
            repair.Diagnosis = repairRequest.Diagnosis;
            repair.AddressId = repairRequest.CurrentAddressId;

            item.AddressId = repairRequest.CurrentAddressId;
            item.Status = DeviceStatus.InStock;

            await _repository.Update(item);
            await _repairRepository.Update(repair); 

            return Result.Ok();
        }
        catch (InvalidOperationException ex)
        {
            return Result.Fail(new InvalidOperationError(ex.Message));
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

    public async Task<Result<byte[]>> ExportItemsToExcel(ItemFilter filter)
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
            var repair = await _repairRepository.GetRepairByItemId(repairId);

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
                    doc.InsertParagraph($"Результаты диагностики: {repair.Diagnosis}").Bold();
                    doc.InsertParagraph();
                }

                doc.InsertParagraph($"Статус ремонта: {repair.Status.GetDisplayName()}").Bold();
                doc.InsertParagraph();

                doc.InsertParagraph($"Ответственный за ремонт: {repair.User.FullName}").Bold();
                doc.InsertParagraph();

                doc.Save();

                return Result.Ok(stream.ToArray());
            }
        }
        catch (NullReferenceException ex)
        {
            return Result.Fail(new NotFoundError(ex.Message));
        }
        catch (Exception)
        {
            return Result.Fail(new Error("Произошла ошибка при генерации документа!"));
        }
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