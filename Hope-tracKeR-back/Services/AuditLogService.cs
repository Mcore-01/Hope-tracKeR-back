using System.Security.Claims;
using System.Text.Json;
using Hope_tracKeR_back.Config;
using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class AuditLogService : IAuditLogService
{
    private readonly HTContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuditLogService> _logger;
    public AuditLogService(HTContext context, IHttpContextAccessor httpContextAccessor, ILogger<AuditLogService> logger)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task LogAsync(string action, string entityName, string entityId, object? oldValues, object? newValues)
    {
        try
        {
            var auditLog = new AuditLog
            {
                Date = DateTime.UtcNow,
                UserLogin = GetCurrentUserLogin(),
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                OldValues = Serialize(oldValues),
                NewValues = Serialize(newValues) ?? "{}"
            };

            await _context.AuditLogs.AddAsync(auditLog);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Audit: {Action} {EntityName} {EntityId} by {UserLogin}", action, entityName, entityId, auditLog.UserLogin);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Не удалось записать аудит: {Action} {EntityName} {EntityId}", action, entityName, entityId);
        }
    }

    private string GetCurrentUserLogin()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated == true)
            return user.FindFirst("login")?.Value ?? user.FindFirst(ClaimTypes.Name)?.Value  ?? "unknown";
        
        return "system";
    }

    private static string? Serialize(object? value)
    {
        if (value is null)
            return null;

        return JsonSerializer.Serialize(value, JsonConfig.JsonOptions);
    }
}