using FluentValidation;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services.Interfaces;

namespace Hope_tracKeR_back.Services;

public class AddressService : BaseCatalogService<Address>
{
    public AddressService(ICatalogRepository<Address> repository, IValidator<Address> validator, IAuditLogService auditLog)
        : base(repository, validator, auditLog) { }
}