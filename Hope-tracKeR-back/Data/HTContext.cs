using Hope_tracKeR_back.Enums;
using Hope_tracKeR_back.Models;
using Microsoft.EntityFrameworkCore;

namespace Hope_tracKeR_back.Data;

public class HTContext(DbContextOptions<HTContext> options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Item>()
            .Property(e => e.AddedDate)
            .HasConversion(
                v => v.ToUniversalTime(),
                v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
            );

        modelBuilder.Entity<Brand>().HasData(
            new Brand { Id = 1, Name = "Samsung" },
            new Brand { Id = 2, Name = "Dexp" },
            new Brand { Id = 3, Name = "LG" },
            new Brand { Id = 4, Name = "Logitech" }
        );

        modelBuilder.Entity<Address>().HasData(
            new Address { Id = 1, Branch = "ул. Пушкина 1", Building = "Корпус 1", Floor = 1, Room = "Кабинет 101"},     
            new Address { Id = 2, Branch = "ул. Пушкина 1", Building = "Корпус 1", Floor = 1, Room = "Кабинет 102"},      
            new Address { Id = 3, Branch = "ул. Толстого 31", Building = "Корпус 4", Floor = 3, Room = "Кабинет 314" }     
        );

        modelBuilder.Entity<Item>().HasData(
            new Item { Id = 1, Name = "Монитор Samsung Odyssey", SerialId = "SAMS-OD-001", Category = ItemCategory.Technique, Status = ItemStatus.InStock, AddedDate = new DateTime(2024, 1, 15), AddressId = 1, BrandId = 1 },
            new Item { Id = 2, Name = "Ноутбук LG Gram", SerialId = "LG-GRAM-002", Category = ItemCategory.Technique, Status = ItemStatus.Issued, AddedDate = new DateTime(2024, 2, 10), AddressId = 2, BrandId = 3 },
            new Item { Id = 3, Name = "Клавиатура Logitech MX", SerialId = "LOG-MX-003", Category = ItemCategory.Technique, Status = ItemStatus.InStock, AddedDate = new DateTime(2024, 3, 5), AddressId = 1, BrandId = 4 },
            new Item { Id = 4, Name = "Бумага A4 500л", SerialId = "PAP-A4-004", Category = ItemCategory.Consumables, Status = ItemStatus.InStock, AddedDate = new DateTime(2024, 3, 20), AddressId = 3, BrandId = 2 },
            new Item { Id = 5, Name = "Картридж для принтера", SerialId = "CRTG-005", Category = ItemCategory.Consumables, Status = ItemStatus.InStock, AddedDate = new DateTime(2024, 4, 1), AddressId = 2, BrandId = 1 },
            new Item { Id = 6, Name = "USB Flash Drive 32GB", SerialId = "USB-32-006", Category = ItemCategory.Consumables, Status = ItemStatus.Repair, AddedDate = new DateTime(2024, 1, 25), AddressId = 1, BrandId = 2 }
        );

        modelBuilder.Entity<ItemAttribute>().HasData(
            new ItemAttribute { Id = 1, ItemId = 1, Name = "Диагональ", Value = "27 дюмов" },
            new ItemAttribute { Id = 2, ItemId = 1, Name = "Разрешение", Value = "2560x1440" },
            new ItemAttribute { Id = 3, ItemId = 1, Name = "Герцовка", Value = "165Hz" }
        );
    }

    public DbSet<Brand> Brands { get; set; }
    public DbSet<Address> Addresses { get; set; }   
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemAttribute> ItemAttributes { get; set; }
}