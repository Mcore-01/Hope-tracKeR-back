using Hope_tracKeR_back.Enums;
using Hope_tracKeR_back.Models.Entities;
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

        modelBuilder.Entity<Brand>().HasData(
            new Brand { Id = 1, Name = "Samsung" },
            new Brand { Id = 2, Name = "Dexp" },
            new Brand { Id = 3, Name = "LG" },
            new Brand { Id = 4, Name = "Logitech" }
        );

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, FullName = "Рустам Вордович", Login = "word365", Password = "admin", Role = UserRole.Admin },
            new User { Id = 2, FullName = "Родион Экселович", Login = "excel2003", Password = "admin", Role = UserRole.Admin },
            new User { Id = 3, FullName = "Моисей Шарпович", Login = "sharp3000", Password = "admin", Role = UserRole.Admin },
            new User { Id = 4, FullName = "Михаил Реактович", Login = "react2026", Password = "admin", Role = UserRole.Admin },
            new User { Id = 5, FullName = "Кирилл TracKeR", Login = "legenda212", Password = "hope", Role = UserRole.Employee }
        );

        modelBuilder.Entity<Address>().HasData(
            new Address { Id = 1, Branch = "ул. Пушкина 1", Building = "Корпус 1", Floor = 1, Room = "Кабинет 101" },
            new Address { Id = 2, Branch = "ул. Пушкина 1", Building = "Корпус 1", Floor = 1, Room = "Кабинет 102" },
            new Address { Id = 3, Branch = "ул. Толстого 31", Building = "Корпус 4", Floor = 3, Room = "Кабинет 314" }
        );

        modelBuilder.Entity<Item>().HasData(
            new Item { Id = 1, Name = "Монитор Samsung Odyssey", SerialId = "SAMS-OD-001", Category = ItemCategory.Technique, Status = ItemStatus.InStock, AddedDate = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc), AddressId = 1, BrandId = 1 },
            new Item { Id = 2, Name = "Ноутбук LG Gram", SerialId = "LG-GRAM-002", Category = ItemCategory.Technique, Status = ItemStatus.Issued, AddedDate = new DateTime(2024, 2, 10, 0, 0, 0, DateTimeKind.Utc), AddressId = 2, BrandId = 3 },
            new Item { Id = 3, Name = "Клавиатура Logitech MX", SerialId = "LOG-MX-003", Category = ItemCategory.Technique, Status = ItemStatus.InStock, AddedDate = new DateTime(2024, 3, 5, 0, 0, 0, DateTimeKind.Utc), AddressId = 1, BrandId = 4 },
            new Item { Id = 4, Name = "Бумага A4 500л", SerialId = "PAP-A4-004", Category = ItemCategory.Consumables, Status = ItemStatus.InStock, AddedDate = new DateTime(2024, 3, 20, 0, 0, 0, DateTimeKind.Utc), AddressId = 3, BrandId = 2 },
            new Item { Id = 5, Name = "Картридж для принтера", SerialId = "CRTG-005", Category = ItemCategory.Consumables, Status = ItemStatus.InStock, AddedDate = new DateTime(2024, 4, 1, 0, 0, 0, DateTimeKind.Utc), AddressId = 2, BrandId = 1 },
            new Item { Id = 6, Name = "USB Flash Drive 32GB", SerialId = "USB-32-006", Category = ItemCategory.Consumables, Status = ItemStatus.Repair, AddedDate = new DateTime(2024, 1, 25, 0, 0, 0, DateTimeKind.Utc), AddressId = 1, BrandId = 2 }
        );

        modelBuilder.Entity<ItemAttribute>().HasData(
            new ItemAttribute { Id = 1, ItemId = 1, Name = "Диагональ", Value = "27 дюмов" },
            new ItemAttribute { Id = 2, ItemId = 1, Name = "Разрешение", Value = "2560x1440" },
            new ItemAttribute { Id = 3, ItemId = 1, Name = "Герцовка", Value = "165Hz" }
        );

        modelBuilder.Entity<Repair>().HasData(
            new Repair { Id = 1, StartDate = new DateTime(2025, 5, 10, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2025, 5, 15, 0, 0, 0, DateTimeKind.Utc), Description = "Монитор не включается, индикатор питания не горит", Diagnosis = "Неисправен блок питания, замена конденсаторов", Status = RepairStatus.Completed, ItemId = 1, AddressId = 1 },
            new Repair { Id = 2, StartDate = new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), EndDate = null, Description = "Ноутбук зависает при загрузке Windows", Diagnosis = "Ожидается диагностика", Status = RepairStatus.InProgress, ItemId = 2, AddressId = 2 },
            new Repair { Id = 3, StartDate = new DateTime(2025, 5, 25, 0, 0, 0, DateTimeKind.Utc), EndDate = null, Description = "Клавиатура не печатает буквы, некоторые кнопки залипли", Diagnosis = "Механическое повреждение, требуется чистка", Status = RepairStatus.InProgress, ItemId = 3, AddressId = 1 },
            new Repair { Id = 4, StartDate = new DateTime(2025, 5, 5, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2025, 5, 8, 0, 0, 0, DateTimeKind.Utc), Description = "Флешка не определяется компьютером", Diagnosis = "Сбой контроллера, данные восстановлены", Status = RepairStatus.Completed, ItemId = 6, AddressId = 1 },
            new Repair { Id = 5, StartDate = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = null, Description = "Монитор моргает и периодически гаснет", Diagnosis = "Неисправен шлейф матрицы", Status = RepairStatus.InProgress, ItemId = 1, AddressId = 1 },
            new Repair { Id = 6, StartDate = new DateTime(2025, 6, 3, 0, 0, 0, DateTimeKind.Utc), EndDate = null, Description = "Ноутбук сильно греется и выключается", Diagnosis = "Ожидается диагностика", Status = RepairStatus.InProgress, ItemId = 2, AddressId = 2 }
        );
    }

    public DbSet<Brand> Brands { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<ItemAttribute> ItemAttributes { get; set; }
    public DbSet<Repair> Repairs { get; set; }
}