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

        modelBuilder.Entity<Brand>()
            .HasIndex(b => b.Name)
            .IsUnique();

        modelBuilder.Entity<Brand>().HasData(
            new Brand { Id = 1, Name = "Samsung" },
            new Brand { Id = 2, Name = "Dexp" },
            new Brand { Id = 3, Name = "LG" },
            new Brand { Id = 4, Name = "Logitech" },
            new Brand { Id = 5, Name = "Svetocopy" },
            new Brand { Id = 6, Name = "Kite" },
            new Brand { Id = 7, Name = "Attache" },
            new Brand { Id = 8, Name = "OfficeSpace" }
        );

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Монитор" },
            new Category { Id = 2, Name = "Ноутбук" },
            new Category { Id = 3, Name = "Картридж" },
            new Category { Id = 4, Name = "Моноблок" }
        );

        modelBuilder.Entity<Employee>().HasData(
            new Employee { Id = 1, FullName = "Петров А.А.", Staff = "Врач" },
            new Employee { Id = 2, FullName = "Сидоров С.С.", Staff = "Заведующий хоз. товарами" },
            new Employee { Id = 3, FullName = "Иванов Д.Д.", Staff = "Мастер над монетой" }
        ); 

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, FullName = "Рустам Вордович", Login = "word365", Password = "21232F297A57A5A743894A0E4A801FC3" },
            new User { Id = 2, FullName = "Родион Экселович", Login = "excel2003", Password = "21232F297A57A5A743894A0E4A801FC3" },
            new User { Id = 3, FullName = "Моисей Шарпович", Login = "sharp3000", Password = "21232F297A57A5A743894A0E4A801FC3" },
            new User { Id = 4, FullName = "Михаил Реактович", Login = "react2026", Password = "21232F297A57A5A743894A0E4A801FC3" },
            new User { Id = 5, FullName = "Кирилл TracKeR", Login = "legenda212", Password = "8C728E685DDDE9F7FBBC452155E29639" }
        );

        modelBuilder.Entity<Address>().HasData(
            new Address { Id = 1, Branch = "ул. Пушкина 1", Building = "Корпус 1", Floor = 1, Room = "Кабинет 101" },
            new Address { Id = 2, Branch = "ул. Пушкина 1", Building = "Корпус 1", Floor = 1, Room = "Кабинет 102" },
            new Address { Id = 3, Branch = "ул. Толстого 31", Building = "Корпус 4", Floor = 3, Room = "Кабинет 314" }
        );

        modelBuilder.Entity<Device>().HasData(
            new Device { Id = 1, Name = "Монитор Samsung Odyssey", SerialNumber = "SAMS-OD-001", Status = DeviceStatus.InStock, AddedDate = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc), AddressId = 1, BrandId = 1, CategoryId = 1 },
            new Device { Id = 2, Name = "Ноутбук LG Gram", SerialNumber = "LG-GRAM-002", Status = DeviceStatus.Issued, AddedDate = new DateTime(2024, 2, 10, 0, 0, 0, DateTimeKind.Utc), AddressId = 2, BrandId = 3, CategoryId = 2, EmployeeId = 1 },
            new Device { Id = 3, Name = "Моноблок Logitech MX", SerialNumber = "LOG-MX-003", Status = DeviceStatus.InStock, AddedDate = new DateTime(2024, 3, 5, 0, 0, 0, DateTimeKind.Utc), AddressId = 1, BrandId = 4, CategoryId = 4 },
            new Device { Id = 5, Name = "Картридж для принтера", SerialNumber = "CRTG-005", Status = DeviceStatus.InStock, AddedDate = new DateTime(2024, 4, 1, 0, 0, 0, DateTimeKind.Utc), AddressId = 2, BrandId = 1, CategoryId = 3 }
        );

        modelBuilder.Entity<Consumable>().HasData(
            new Consumable { Id = 6, Name = "Бумага А4 Svetocopy", AddressId = 1, BrandId = 5, Quantity = 50 },
            new Consumable { Id = 7, Name = "Бумага А3 Svetocopy", AddressId = 1, BrandId = 5, Quantity = 20 },
            new Consumable { Id = 8, Name = "Скрепки 25 мм", AddressId = 2, BrandId = 6, Quantity = 100 },
            new Consumable { Id = 9, Name = "Папка-регистратор Attache", AddressId = 3, BrandId = 7, Quantity = 30 },
            new Consumable { Id = 10, Name = "Ручка шариковая OfficeSpace", AddressId = 2, BrandId = 8, Quantity = 200 },
            new Consumable { Id = 11, Name = "Стикеры самоклеящиеся", AddressId = 1, BrandId = 7, Quantity = 15 }
        );

        modelBuilder.Entity<Cartridge>().HasData(
            new Cartridge { Id = 100, Name = "Тонер-картридж Samsung ML-1660", AddressId = 1, BrandId = 1, Status = CartridgeStatus.InStock, PrinterModel = "ML-1660" },
            new Cartridge { Id = 101, Name = "Тонер-картридж Samsung SCX-4521F", AddressId = 1, BrandId = 1, Status = CartridgeStatus.Empty, PrinterModel = "SCX-4521F" },
            new Cartridge { Id = 102, Name = "Картридж LG Printronix", AddressId = 1, BrandId = 3, Status = CartridgeStatus.Refilling, PrinterModel = "Printronix" },
            new Cartridge { Id = 103, Name = "Тонер Dexp DPP-250", AddressId = 2, BrandId = 2, Status = CartridgeStatus.Installed, PrinterModel = "DPP-250" },
            new Cartridge { Id = 104, Name = "Картридж Samsung CLP-315", AddressId = 2, BrandId = 1, Status = CartridgeStatus.Installed, PrinterModel = "CLP-315" },
            new Cartridge { Id = 105, Name = "Картридж LG LBP-1210", AddressId = 3, BrandId = 3, Status = CartridgeStatus.Installed, PrinterModel = "LBP-1210" }
        );

        modelBuilder.Entity<ItemAttribute>().HasData(
            new ItemAttribute { Id = 1, ItemId = 1, Name = "Диагональ", Value = "27 дюмов" },
            new ItemAttribute { Id = 2, ItemId = 1, Name = "Разрешение", Value = "2560x1440" },
            new ItemAttribute { Id = 3, ItemId = 1, Name = "Герцовка", Value = "165Hz" }
        );

        modelBuilder.Entity<Repair>().HasData(
            new Repair { Id = 1, StartDate = new DateTime(2025, 5, 10, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2025, 5, 15, 0, 0, 0, DateTimeKind.Utc), Description = "Монитор не включается, индикатор питания не горит", Diagnosis = "Неисправен блок питания, замена конденсаторов", Status = RepairStatus.Completed, ItemId = 1, AddressId = 1, UserId = 1 },
            new Repair { Id = 2, StartDate = new DateTime(2025, 5, 20, 0, 0, 0, DateTimeKind.Utc), EndDate = null, Description = "Ноутбук зависает при загрузке Windows", Diagnosis = "Ожидается диагностика", Status = RepairStatus.InProgress, ItemId = 2, AddressId = 2, UserId = 1 },
            new Repair { Id = 3, StartDate = new DateTime(2025, 5, 25, 0, 0, 0, DateTimeKind.Utc), EndDate = null, Description = "Клавиатура не печатает буквы, некоторые кнопки залипли", Diagnosis = "Механическое повреждение, требуется чистка", Status = RepairStatus.InProgress, ItemId = 3, AddressId = 1, UserId = 1 },
            new Repair { Id = 4, StartDate = new DateTime(2025, 5, 5, 0, 0, 0, DateTimeKind.Utc), EndDate = new DateTime(2025, 5, 8, 0, 0, 0, DateTimeKind.Utc), Description = "Флешка не определяется компьютером", Diagnosis = "Сбой контроллера, данные восстановлены", Status = RepairStatus.Completed, ItemId = 6, AddressId = 1, UserId = 1 },
            new Repair { Id = 5, StartDate = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc), EndDate = null, Description = "Монитор моргает и периодически гаснет", Diagnosis = "Неисправен шлейф матрицы", Status = RepairStatus.InProgress, ItemId = 1, AddressId = 1, UserId = 1 },
            new Repair { Id = 6, StartDate = new DateTime(2025, 6, 3, 0, 0, 0, DateTimeKind.Utc), EndDate = null, Description = "Ноутбук сильно греется и выключается", Diagnosis = "Ожидается диагностика", Status = RepairStatus.InProgress, ItemId = 2, AddressId = 2, UserId = 1 }
        );
    }

    public DbSet<Brand> Brands { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Consumable> Consumables { get; set; }
    public DbSet<Cartridge> Cartridges { get; set; }
    public DbSet<ItemAttribute> ItemAttributes { get; set; }
    public DbSet<Repair> Repairs { get; set; }
    public DbSet<WriteOff> WriteOffs { get; set; }
    public DbSet<Issuance> Issuances { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
}