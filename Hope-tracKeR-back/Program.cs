using FluentValidation;
using Hope_tracKeR_back.Config;
using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Middlewares;
using Hope_tracKeR_back.Models.DTOs.Requests;
using Hope_tracKeR_back.Models.DTOs.Responses;
using Hope_tracKeR_back.Models.Entities;
using Hope_tracKeR_back.Repositories;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services;
using Hope_tracKeR_back.Services.Interfaces;
using Hope_tracKeR_back.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddDbContext<HTContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
services.AddAutoMapper(typeof(Program));
services.AddValidatorsFromAssemblyContaining<BrandValidator>();
services.AddValidatorsFromAssemblyContaining<AddressValidator>();
services.AddValidatorsFromAssemblyContaining<EmployeeValidator>();
services.AddValidatorsFromAssemblyContaining<CategoryValidator>();
services.AddScoped<ICatalogRepository<Brand>, BrandRepository>();
services.AddScoped<ICatalogRepository<Address>, AddressRepository>();
services.AddScoped<ICatalogRepository<Employee>, EmployeeRepository>();
services.AddScoped<ICatalogRepository<Category>, CategoryRepository>();
services.AddScoped<IItemRepository<Device>, DeviceRepository>();
services.AddScoped<IItemRepository<Consumable>, ConsumableRepository>();
services.AddScoped<IWriteOffRepository, WriteOffRepository>();
services.AddScoped<IIssuanceRepository, IssuanceRepository>();
services.AddScoped<IRepairRepository, RepairRepository>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<ICatalogService<Brand>, BrandService>();
services.AddScoped<ICatalogService<Address>, AddressService>();
services.AddScoped<ICatalogService<Employee>, EmployeeService>();
services.AddScoped<ICatalogService<Category>, CategoryService>();
services.AddScoped<IRepairService, RepairService>();
services.AddScoped<IWriteOffService, WriteOffService>();
services.AddScoped<IIssuanceService, IssuanceService>();
services.AddScoped<IItemService<DeviceRequest, DeviceResponse>, DeviceService>();
services.AddScoped<IItemService<ConsumableRequest, ConsumableResponse>, ConsumableService>();
services.AddScoped<IAuthService, AuthService>();
services.AddAuthorization();
services.ConfigureCors();
services.ConfigureAuthentication();
services.AddSwaggerGen();
services.AddControllers();
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HTContext>();
    context.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<LogMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();