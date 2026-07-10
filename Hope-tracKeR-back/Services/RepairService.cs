using AutoMapper;
using FluentResults;
using FluentValidation;
using Hope_tracKeR_back.Constants;
using Hope_tracKeR_back.Enums;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Hope_tracKeR_back.Services;

public class RepairService : IRepairService
{
    private readonly IRepairRepository _repairRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<StartRepairRequest> _startRepairValidator;
    private readonly IValidator<CompleteRepairRequest> _completeRepairValidator;
    private readonly IAuditLogService _auditLog;

    public RepairService(IMapper mapper, IValidator<StartRepairRequest> startRepairValidator,
        IValidator<CompleteRepairRequest> completeRepairValidator, IRepairRepository repairRepository, IAuditLogService auditLog)
    {
        _mapper = mapper;
        _startRepairValidator = startRepairValidator;
        _completeRepairValidator = completeRepairValidator;
        _repairRepository = repairRepository;
        _auditLog = auditLog;
    }

    public async Task<Result<int>> StartRepair(StartRepairRequest repairRequest)
    {
        var validationResult = await _startRepairValidator.ValidateAsync(repairRequest);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("\n", validationResult.Errors);
            return Result.Fail<int>(new ValidationError(errors));
        }

        try
        {
            var repair = _mapper.Map<Repair>(repairRequest);
            repair.Status = RepairStatus.InProgress;

            var item = await _repairRepository.GetDeviceById(repair.ItemId);
            var oldDevice = new { item.Id, item.Status, item.AddressId };

            item.Status = DeviceStatus.Repair;
            item.AddressId = repair.AddressId;

            await _repairRepository.UpdateDevice(item);
            await _repairRepository.Create(repair);
            await _repairRepository.SaveChangesAsync();

            await _auditLog.LogAsync(AuditActions.StartRepair, nameof(Repair), repair.Id.ToString(), repair);
            await _auditLog.LogAsync(AuditActions.Update, nameof(Device), item.Id.ToString(), new { item.Id, item.Status, item.AddressId });

            return Result.Ok(repair.Id);
        }
        catch (InvalidOperationException ex)
        {
            return Result.Fail<int>(new InvalidOperationError(ex.Message));
        }
        catch (NullReferenceException ex)
        {
            return Result.Fail<int>(new NotFoundError(ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Fail<int>(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public async Task<Result> CompleteRepair(CompleteRepairRequest repairRequest)
    {
        var validationResult = await _completeRepairValidator.ValidateAsync(repairRequest);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("\n", validationResult.Errors);
            return Result.Fail(new ValidationError(errors));
        }

        try
        {
            var item = await _repairRepository.GetDeviceById(repairRequest.ItemId);
            var repair = await _repairRepository.GetRepairByItemId(repairRequest.ItemId);

            var oldDevice = new { item.Id, item.Status, item.AddressId };
            var oldRepair = new { repair.Id, repair.Status, repair.EndDate, repair.Diagnosis, repair.AddressId };

            repair.Status = RepairStatus.Completed;
            repair.EndDate = repairRequest.EndDate;
            repair.Diagnosis = repairRequest.Diagnosis;
            repair.AddressId = repairRequest.CurrentAddressId;

            item.AddressId = repairRequest.CurrentAddressId;
            item.Status = DeviceStatus.InStock;

            await _repairRepository.UpdateDevice(item);
            await _repairRepository.Update(repair);
            await _repairRepository.SaveChangesAsync();

            await _auditLog.LogAsync(AuditActions.CompleteRepair, nameof(Repair), repair.Id.ToString(), repair);
            await _auditLog.LogAsync(AuditActions.Update, nameof(Device), item.Id.ToString(), new { item.Id, item.Status, item.AddressId });

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

    public async Task<Result<byte[]>> GenerateRepairActToDocx(int repairId)
    {
        try
        {
            var repair = await _repairRepository.GetRepairByItemId(repairId);

            using var stream = new MemoryStream();

            using var doc = DocX.Create(stream);
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
        catch (NullReferenceException ex)
        {
            return Result.Fail(new NotFoundError(ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Произошла ошибка при генерации документа!"));
        }
    }
}