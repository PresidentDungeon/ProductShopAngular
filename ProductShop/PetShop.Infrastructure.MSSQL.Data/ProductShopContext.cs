using Microsoft.EntityFrameworkCore;
using PetShop.Core.Entities;

namespace PetShop.Infrastructure.SQLLite.Data
{
    public class ProductShopContext : DbContext
    {
        public ProductShopContext(DbContextOptions<ProductShopContext> option) : base(option) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public DbSet<Product> Products {get;set;}
        public DbSet<User> Users { get; set; }
    }
}