using AutoMapper;
using FluentResults;
using FluentValidation;
using Hope_tracKeR_back.Constants;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public abstract class BaseItemService<TEntity, TRequest, TResponse> : IItemService<TRequest, TResponse> where TEntity : Item
{
    protected readonly IItemRepository<TEntity> _repository;
    protected readonly IMapper _mapper;
    protected readonly IValidator<TRequest> _validator;
    protected readonly IAuditLogService _auditLog;

    protected BaseItemService(IItemRepository<TEntity> repository, IMapper mapper, IValidator<TRequest> validator, IAuditLogService auditLog)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
        _auditLog = auditLog;
    }

    public virtual async Task<Result<int>> Create(TRequest itemModify)
    {
        var validationResult = await _validator.ValidateAsync(itemModify);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("\n", validationResult.Errors);
            return Result.Fail<int>(new ValidationError(errors));
        }

        try
        {
            var entity = _mapper.Map<TEntity>(itemModify);
            var id = await _repository.Create(entity);
            await _auditLog.LogAsync(AuditActions.Create, nameof(TEntity), id.ToString(), null, entity);
            return Result.Ok(id);
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

    public virtual async Task<Result<IEnumerable<TResponse>>> GetByFilters(ItemFilter filter)
    {
        try
        {
            var items = await _repository.GetByFilters(filter);
            var response = items.Select(_mapper.Map<TResponse>);
            return Result.Ok(response);
        }
        catch (Exception ex)
        {
            return Result.Fail<IEnumerable<TResponse>>(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public virtual async Task<Result<TResponse>> GetById(int id)
    {
        try
        {
            var entity = await _repository.GetById(id);
            if (entity == null)
            {
                return Result.Fail<TResponse>(new NotFoundError(nameof(TEntity), id));
            }

            var response = _mapper.Map<TResponse>(entity);
            return Result.Ok(response);
        }
        catch (NullReferenceException ex)
        {
            return Result.Fail<TResponse>(new NotFoundError(ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Fail<TResponse>(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public virtual async Task<Result> Update(TRequest itemModify)
    {
        var validationResult = await _validator.ValidateAsync(itemModify);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("\n", validationResult.Errors);
            return Result.Fail(new ValidationError(errors));
        }

        try
        {
            var entity = _mapper.Map<TEntity>(itemModify);
            TEntity? oldEntity = null;
            try
            {
                oldEntity = await _repository.GetById(entity.Id);
            }
            catch (NullReferenceException) { }

            await _repository.Update(entity);
            await _auditLog.LogAsync(AuditActions.Update, nameof(TEntity), entity.Id.ToString(), oldEntity, entity);
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

    public virtual async Task<Result> Remove(int id)
    {
        try
        {
            TEntity? oldEntity = null;
            try
            {
                oldEntity = await _repository.GetById(id);
            }
            catch (NullReferenceException) { }

            await _repository.Remove(id);
            await _auditLog.LogAsync(AuditActions.Delete, nameof(TEntity), id.ToString(), oldEntity, null);
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

    public abstract Task<Result<byte[]>> ExportItemsToExcel(ItemFilter filter);
}