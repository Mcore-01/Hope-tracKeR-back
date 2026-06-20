using FluentValidation;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class CategoryService : BaseCatalogService<Category>
{
    public CategoryService(ICatalogRepository<Category> repository, IValidator<Category> validator, IAuditLogService auditLog)
        : base(repository, validator, auditLog) { }
}