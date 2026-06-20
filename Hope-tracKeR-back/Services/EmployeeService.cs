using FluentValidation;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class EmployeeService : BaseCatalogService<Employee>
{
    public EmployeeService(ICatalogRepository<Employee> repository, IValidator<Employee> validator, IAuditLogService auditLog)
        : base(repository, validator, auditLog) { }
}