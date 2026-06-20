using FluentValidation;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class BrandService : BaseCatalogService<Brand>
{
    public BrandService(ICatalogRepository<Brand> repository, IValidator<Brand> validator, IAuditLogService auditLog)
        : base(repository, validator, auditLog) { }
}