using ShopAPI.Models;
using Microsoft.EntityFrameworkCore;
using ShopAPI.Models;
using System.Reflection.Metadata;

namespace ShopAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryProduct> CategoriesProducts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrdersProducts { get; set;}
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategoryProduct>()
                .HasKey(pc => new { pc.CategoryId, pc.ProductId });
            modelBuilder.Entity<CategoryProduct>()
                .HasOne(p => p.Category)
                .WithMany(pc => pc.CategoryProducts)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CategoryProduct>()
                .HasOne(p => p.Product)
                .WithMany(pc => pc.CategoryProducts)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderProduct>()
                .HasKey(pc => new { pc.OrderId, pc.ProductId });
            modelBuilder.Entity<OrderProduct>()
                .HasOne(p => p.Order)
                .WithMany(pc => pc.OrderProducts)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderProduct>()
                .HasOne(p => p.Product)
                .WithMany(pc => pc.OrderProducts)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRole>()
                .HasKey(pc => new { pc.UserId, pc.RoleId });
            modelBuilder.Entity<UserRole>()
                .HasOne(p => p.User)
                .WithMany(pc => pc.UserRoles)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<UserRole>()
                .HasOne(p => p.Role)
                .WithMany(pc => pc.UserRoles)
                .HasForeignKey(p => p.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
