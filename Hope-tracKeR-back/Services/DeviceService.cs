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

namespace Hope_tracKeR_back.Services;

public class DeviceService : IItemService<DeviceRequest, DeviceResponse>
{
    private readonly IItemRepository<Device> _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<DeviceRequest> _validator;
    public DeviceService(IItemRepository<Device> itemRepository, 
        IMapper mapper, IValidator<DeviceRequest> validator, IValidator<StartRepairRequest> startRepairValidator,
        IValidator<CompleteRepairRequest> completeRepairValidator, IRepairRepository repairRepository)
    {
        _repository = itemRepository;
        _mapper = mapper;
        _validator = validator;
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
}