using Microsoft.EntityFrameworkCore;
using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data.Repository.Products
{
    public class ProductsEfRepository : IProductsRepository
    {
        private readonly AppDbContext _context;

        public ProductsEfRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Product> GetAll()
        {
            return _context.Products.AsNoTracking().ToList();
        }

        public Product TryGetById(int id)
        {
            return _context.Products.AsNoTracking().FirstOrDefault(p => p.Id == id);
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges(); // ← для продакшена лучше async + транзакции
        }

        public void Update(Product product)
        {
            var existing = _context.Products.Find(product.Id);
            if (existing == null) return;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Cost = product.Cost;
            existing.ImagePath = product.ImagePath;

            _context.SaveChanges();
        }
    }
}