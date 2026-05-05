using Microsoft.EntityFrameworkCore;
using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<UserDeliveryInfo> UserDeliveryInfos { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Areas.Admin.Models.Role> Roles { get; set; }
        public DbSet<ProductKey> ProductKeys { get; set; }
        public DbSet<Favorite> Favorites { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product (уже было)
            modelBuilder.Entity<Product>(e =>
            {
                e.HasKey(p => p.Id);
                e.Property(p => p.Id).ValueGeneratedOnAdd();
                e.Property(p => p.Name).IsRequired().HasMaxLength(100);
                e.Property(p => p.Description).HasMaxLength(1000);
                e.Property(p => p.Cost).HasColumnType("decimal(18,2)");
            });

            // Cart & CartItem
            modelBuilder.Entity<CartItem>(e =>
            {
                e.HasKey(i => i.Id);
                e.HasOne(i => i.Cart).WithMany(c => c.Items).HasForeignKey(i => i.CartId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(i => i.Product).WithMany().HasForeignKey(i => i.ProductId).OnDelete(DeleteBehavior.Restrict);
            });

            // Order & OrderItem & User (DeliveryInfo)
            modelBuilder.Entity<Order>(e =>
            {
                e.HasKey(o => o.Id);
                // 👇 Исправлено: используем свойство "User", как в вашем контроллере
                e.HasOne(o => o.User)
                  .WithMany()
                  .HasForeignKey(o => o.DeliveryInfoId)
                  .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OrderItem>(e =>
            {
                e.HasKey(oi => oi.Id);
                e.HasOne(oi => oi.Order).WithMany(o => o.Items).HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(oi => oi.Product).WithMany().HasForeignKey(oi => oi.ProductId).OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<OrderItem>(e =>
            {
                e.HasKey(oi => oi.Id);
                e.HasOne(oi => oi.Order).WithMany(o => o.Items).HasForeignKey(oi => oi.OrderId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(oi => oi.Product).WithMany().HasForeignKey(oi => oi.ProductId).OnDelete(DeleteBehavior.Restrict);
            });

            // ProductKey
            modelBuilder.Entity<ProductKey>(e =>
            {
                e.HasKey(k => k.Id);
                e.Property(k => k.Key).IsRequired().HasMaxLength(29);
                e.HasOne(k => k.Product).WithMany().HasForeignKey(k => k.ProductId).OnDelete(DeleteBehavior.Restrict);
            });

            // UserAccount → Role
            modelBuilder.Entity<UserAccount>(e =>
            {
                e.HasKey(u => u.Id);
                e.Property(u => u.RoleId).HasDefaultValue(1);
                e.HasOne(u => u.Role).WithMany().HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Favorite>(e =>
            {
                e.HasKey(f => f.Id);
                e.Property(f => f.UserId).IsRequired().HasMaxLength(256);
                e.Property(f => f.GameName).IsRequired().HasMaxLength(200);
                e.Property(f => f.GameImageUrl).HasMaxLength(500);
                e.HasIndex(f => new { f.UserId, f.RawgGameId }).IsUnique();
            });

            modelBuilder.Entity<UserDeliveryInfo>().HasKey(d => d.Id);
            modelBuilder.Entity<Areas.Admin.Models.Role>().HasKey(r => r.Id);

            // 🌱 Сидирование (оставьте только для Products, остальные заполнятся через приложение)
            SeedInitialData(modelBuilder);
        }

        private void SeedInitialData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Areas.Admin.Models.Role>().HasData(
                new Areas.Admin.Models.Role { Id = 1, Name = "User" },
                new Areas.Admin.Models.Role { Id = 2, Name = "Admin" }
            );

            modelBuilder.Entity<UserAccount>().HasData(
                new UserAccount { Id = 9999, Name = "admin@steamkooper.ru", Password = "admin", RoleId = 2 }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product("Elden Ring", 1999, "...", "/images/Elden_Ring_Logo.jpg") { Id = 1 },
                new Product("Cyberpunk 2077", 1299, "...", "/images/Cyberpunk_2077_Logo.jpg") { Id = 2 },
                new Product("Baldur's Gate 3", 1999, "...", "/images/Baldurs_Gate_3_Logo.jpg") { Id = 3 },
                new Product("Path of Exile 2", 1499, "...", "/images/Path_Of_Exile_2_Logo.jpg") { Id = 4 }
            );
        }
    }
}