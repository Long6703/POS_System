using Microsoft.EntityFrameworkCore;
using POS.Shared.Entities;
using System;

namespace POS_API.DatabaseContext
{
    public class POSSystemDBContext : DbContext
    {
        public POSSystemDBContext()
        {
            
        }

        public POSSystemDBContext(DbContextOptions<POSSystemDBContext> options): base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<UserShop> UserShops { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                optionsBuilder.UseSqlServer(builder.Build().GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserShop>()
                .HasKey(us => new { us.UserId, us.ShopId });

            modelBuilder.Entity<UserShop>()
                .Property(us => us.Role)
                .HasConversion<string>();

            modelBuilder.Entity<OrderDetail>()
                .HasKey(od => od.Id);

            modelBuilder.Entity<Payment>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserShop>()
                .HasOne(us => us.User)
                .WithMany(u => u.UserShops)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserShop>()
                .HasOne(us => us.Shop)
                .WithMany(s => s.UserShops)
                .HasForeignKey(us => us.ShopId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>()
                .Property(us => us.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Payment>()
                .Property(us => us.PaymentMethod)
                .HasConversion<string>();

            modelBuilder.Entity<Order>()
                .Property(us => us.Status)
                .HasConversion<string>();
        }
    }
}
