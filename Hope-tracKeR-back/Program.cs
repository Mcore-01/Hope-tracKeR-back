using FluentValidation;
using Hope_tracKeR_back.Config;
using Hope_tracKeR_back.Data;
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
services.AddValidatorsFromAssemblyContaining<BrandValidator>();
services.AddScoped<ICatalogRepository<Brand>, BrandRepository>();
services.AddScoped<ICatalogRepository<Address>, AddressRepository>();
services.AddScoped<IItemRepository, ItemRepository>();
services.AddScoped<IRepairRepository, RepairRepository>();
services.AddScoped<IUserRepository, UserRepository>();
services.AddScoped<ICatalogService<Brand>, BrandService>();
services.AddScoped<ICatalogService<Address>, AddressService>();
services.AddScoped<IItemService, ItemService>();
services.AddScoped<IAuthService, AuthService>();
services.AddAuthorization();
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
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();