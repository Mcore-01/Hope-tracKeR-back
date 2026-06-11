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

    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
}