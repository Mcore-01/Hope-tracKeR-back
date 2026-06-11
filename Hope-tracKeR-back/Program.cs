using Hope_tracKeR_back.Data;
using Hope_tracKeR_back.Repositories;
using Hope_tracKeR_back.Repositories.Interfaces;
using Hope_tracKeR_back.Services;
using Hope_tracKeR_back.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
services.AddDbContext<HTContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
services.AddScoped<IBrandRepository, BrandRepository>();
services.AddScoped<IItemRepository, ItemRepository>();
services.AddScoped<IAddressRepository, AddressRepository>();
services.AddScoped<IEnumService, EnumService>();
services.AddScoped<IItemService, ItemService>();
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
app.MapControllers();
app.Run();
