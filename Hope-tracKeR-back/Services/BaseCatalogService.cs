using FluentResults;
using FluentValidation;
using Hope_tracKeR_back.Constants;
using Hope_tracKeR_back.Errors;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public abstract class BaseCatalogService<TEntity> : ICatalogService<TEntity> where TEntity : class
{
    protected readonly ICatalogRepository<TEntity> Repository;
    protected readonly IValidator<TEntity> Validator;
    protected readonly IAuditLogService AuditLog;
    protected readonly string EntityName = typeof(TEntity).Name;

    protected BaseCatalogService(ICatalogRepository<TEntity> repository, IValidator<TEntity> validator, IAuditLogService auditLog)
    {
        Repository = repository;
        Validator = validator;
        AuditLog = auditLog;
    }

    public virtual async Task<Result<IEnumerable<TEntity>>> GetAll()
    {
        try
        {
            var items = await Repository.GetAll();
            return Result.Ok(items);
        }
        catch (Exception ex)
        {
            return Result.Fail<IEnumerable<TEntity>>(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public virtual async Task<Result<TEntity>> GetById(int id)
    {
        try
        {
            var item = await Repository.GetById(id);
            if (item is null)
            {
                return Result.Fail<TEntity>(new NotFoundError(EntityName, id));
            }

            return Result.Ok(item);
        }
        catch (NullReferenceException ex)
        {
            return Result.Fail<TEntity>(new NotFoundError(EntityName, id));
        }
        catch (Exception ex)
        {
            return Result.Fail<TEntity>(new Error($"Произошла ошибка: {ex.Message}"));
        }
    }

    public virtual async Task<Result<int>> Create(TEntity entity)
    {
        var validationResult = await Validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("\n", validationResult.Errors);
            return Result.Fail<int>(new ValidationError(errors));
        }

        try
        {
            var id = await Repository.Create(entity);
            await AuditLog.LogAsync(AuditActions.Create, EntityName, id.ToString(), entity);
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

    public virtual async Task<Result> Update(TEntity entity)
    {
        var entityId = GetEntityId(entity);

        var validationResult = await Validator.ValidateAsync(entity);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("\n", validationResult.Errors);
            return Result.Fail(new ValidationError(errors));
        }

        try
        {
  

            await Repository.Update(entity);
            await AuditLog.LogAsync(AuditActions.Update, EntityName, entityId ?? "unknown", entity);
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
                oldEntity = await Repository.GetById(id);
            }
            catch (NullReferenceException) { }

            await Repository.Remove(id);
            await AuditLog.LogAsync(AuditActions.Delete, EntityName, id.ToString(), oldEntity);
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

    private static string? GetEntityId(TEntity entity)
    {
        var idProperty = entity.GetType().GetProperty("Id");
        return idProperty?.GetValue(entity)?.ToString();
    }
}