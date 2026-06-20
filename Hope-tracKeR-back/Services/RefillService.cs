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

public class RefillService : IRefillService
{
    private readonly IItemRepository<Cartridge> _repository;
    private readonly IRefillRepository _refillRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<StartRefillRequest> _startRefillValidator;
    private readonly IValidator<CompleteRefillRequest> _completeRefillValidator;
    private readonly IAuditLogService _auditLog;

    public RefillService(IItemRepository<Cartridge> itemRepository, IMapper mapper, IValidator<StartRefillRequest> startRefillValidator,
        IValidator<CompleteRefillRequest> completeRefillValidator, IRefillRepository refillRepository, IAuditLogService auditLog)
    {
        _repository = itemRepository;
        _mapper = mapper;
        _startRefillValidator = startRefillValidator;
        _completeRefillValidator = completeRefillValidator;
        _refillRepository = refillRepository;
        _auditLog = auditLog;
    }

    public async Task<Result<int>> StartRefill(StartRefillRequest refillRequest)
    {
        var validationResult = await _startRefillValidator.ValidateAsync(refillRequest);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("\n", validationResult.Errors);
            return Result.Fail<int>(new ValidationError(errors));
        }

        try
        {
            var refill = _mapper.Map<Refill>(refillRequest);
            refill.Status = RefillStatus.InProgress;

            var item = await _repository.GetById(refill.ItemId);
            var oldCartridge = new { item.Id, item.Status, item.AddressId };

            item.Status = CartridgeStatus.Refilling;
            item.AddressId = refill.AddressId;

            await _repository.Update(item);

            var refillId = await _refillRepository.Create(refill);

            await _auditLog.LogAsync(AuditActions.StartRefill, nameof(Refill), refillId.ToString(), refill);
            await _auditLog.LogAsync(AuditActions.Update, nameof(Cartridge), item.Id.ToString(), new { item.Id, item.Status, item.AddressId });

            return Result.Ok(refillId);
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

    public async Task<Result> CompleteRefill(CompleteRefillRequest refillRequest)
    {
        var validationResult = await _completeRefillValidator.ValidateAsync(refillRequest);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("\n", validationResult.Errors);
            return Result.Fail(new ValidationError(errors));
        }

        try
        {
            var item = await _repository.GetById(refillRequest.ItemId);
            var refill = await _refillRepository.GetRefillByItemId(refillRequest.ItemId);

            var oldCartridge = new { item.Id, item.Status, item.AddressId };
            var oldRefill = new { refill.Id, refill.Status, refill.EndDate, refill.AddressId };

            refill.Status = RefillStatus.Completed;
            refill.EndDate = refillRequest.EndDate;
            refill.AddressId = refillRequest.AddressId;

            item.AddressId = refillRequest.AddressId;
            item.Status = CartridgeStatus.InStock;

            await _repository.Update(item);
            await _refillRepository.Update(refill);

            await _auditLog.LogAsync(AuditActions.CompleteRefill, nameof(Refill), refill.Id.ToString(), refill);
            await _auditLog.LogAsync(AuditActions.Update, nameof(Cartridge), item.Id.ToString(), new { item.Id, item.Status, item.AddressId });

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

    public async Task<Result<byte[]>> GenerateRefillToDocx(int itemId)
    {
        try
        {
            var refill = await _refillRepository.GetRefillByItemId(itemId);

            using var stream = new MemoryStream();

            using var doc = DocX.Create(stream);
            doc.InsertParagraph("АКТ ПРИЕМА-ПЕРЕДАЧИ").FontSize(16).Bold().Alignment = Alignment.center;

            doc.InsertParagraph("картриджа на заправку").FontSize(14).Bold().Alignment = Alignment.center;

            doc.InsertParagraph($"№ {refill.Id}").FontSize(14).Bold().Alignment = Alignment.center;
            doc.InsertParagraph();

            doc.InsertParagraph($"«{refill.StartDate:dd}» {refill.StartDate.Month.ToString().PadLeft(2, '0')} {refill.StartDate:yyyy} г.");
            doc.InsertParagraph();

            doc.InsertParagraph($"Текущее местоположение: {refill.Address.Branch}, {refill.Address.Building}, {refill.Address.Floor}, {refill.Address.Room}");
            doc.InsertParagraph();

            doc.InsertParagraph("Картридж, передаваемый на заправку:").Bold();

            var table = doc.AddTable(2, 3);

            table.Rows[0].Cells[0].Paragraphs.First().Append("Идентификатор отчета").Bold();
            table.Rows[0].Cells[1].Paragraphs.First().Append("Наименование").Bold();
            table.Rows[0].Cells[2].Paragraphs.First().Append("Модель принтера").Bold();

            table.Rows[1].Cells[0].Paragraphs.First().Append(refill.ItemId.ToString());
            table.Rows[1].Cells[1].Paragraphs.First().Append(refill.Item.Name);
            table.Rows[1].Cells[2].Paragraphs.First().Append(refill.Item.PrinterModel);

            doc.InsertTable(table);
            doc.InsertParagraph();

            doc.InsertParagraph($"Статус заправки: {refill.Status.GetDisplayName()}").Bold();
            doc.InsertParagraph();

            doc.InsertParagraph($"Ответственное лицо: {refill.User.FullName}").Bold();
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