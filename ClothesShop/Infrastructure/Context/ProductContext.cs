using Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Context
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }
        public DbSet<Product> Products { get; set; }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<User>()
               .HasKey(p => p.Id);

            modelBuilder.Entity<Product>().HasIndex(p => p.Name).IsUnique();
        }
    }
}
