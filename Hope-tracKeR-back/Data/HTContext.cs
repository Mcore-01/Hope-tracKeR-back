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

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Техника" },
            new Category { Id = 2, Name = "Расходники" }
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
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Address> Addresses { get; set; }   
}