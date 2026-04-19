using Microsoft.EntityFrameworkCore;
using Multi_Tenant_E_Commerce_API.Models;

namespace Multi_Tenant_E_Commerce_API.Data
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>()
                .Property(c => c.CompanyId)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            modelBuilder.Entity<User>()
                .Property(u => u.UserId)
                .HasDefaultValueSql("NEWSEQUENTIALID()");


            modelBuilder.Entity<Item>()
                .Property(i => i.ItemId)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            modelBuilder.Entity<Cart>()
                .Property(c => c.CartId)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            modelBuilder.Entity<Order>()
                .Property(o => o.OrderId)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Item)
                .WithMany()
                .HasForeignKey(ci => ci.ItemId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany()
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany()
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
