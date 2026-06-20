namespace Hope_tracKeR_back.Services.Interfaces;

public interface IAuditLogService
{
    Task LogAsync(string action, string entityName, string entityId, object? oldValues, object? newValues);
}