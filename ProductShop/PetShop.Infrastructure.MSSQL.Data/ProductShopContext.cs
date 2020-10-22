using Microsoft.EntityFrameworkCore;
using PetShop.Core.Entities;

namespace PetShop.Infrastructure.SQLLite.Data
{
    public class ProductShopContext : DbContext
    {
        public ProductShopContext(DbContextOptions<ProductShopContext> option) : base(option) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Type)
                .WithMany(pt => pt.Products).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<ProductColor>()
                .HasOne(p => p.Product)
                .WithMany(p => p.productColors);

            modelBuilder.Entity<ProductColor>()
                .HasOne(c => c.Color)
                .WithMany(c => c.ProductColors);

            modelBuilder.Entity<ProductColor>().HasKey(p => new { p.ProductID, p.ColorID });
        }

        public DbSet<Product> Products {get;set;}
        public DbSet<User> Users { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
    }
}