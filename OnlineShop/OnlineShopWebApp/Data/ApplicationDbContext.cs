using Microsoft.EntityFrameworkCore;
using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        // Добавьте другие DbSet по мере миграции: UserAccount, Cart, Order...

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd(); // ← авто-инкремент
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.Cost).HasColumnType("decimal(18,2)");
            });

            // 🔥 Сидирование тестовых данных (как у вас в памяти)
            SeedInitialData(modelBuilder);
        }

        private void SeedInitialData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product("Elden Ring", 1999,
                    "Elden Ring — это захватывающая action-RPG...",
                    "/images/Elden_Ring_Logo.jpg")
                { Id = 1 },
                new Product("Cyberpunk 2077", 1299,
                    "Cyberpunk 2077 — это ролевая игра...",
                    "/images/Cyberpunk_2077_Logo.jpg")
                { Id = 2 },
                new Product("Baldur's Gate 3", 1999,
                    "Baldur's Gate 3 — это эпическая ролевая игра...",
                    "/images/Baldurs_Gate_3_Logo.jpg")
                { Id = 3 },
                new Product("Path of Exile 2", 1499,
                    "Path of Exile 2 — это продолжение культовой action-RPG...",
                    "/images/Path_Of_Exile_2_Logo.jpg")
                { Id = 4 }
            );
        }
    }
}