using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data.Repository.Products
{
    public class ProductsInMemoryRepository : IProductsRepository
    {
        private List<Product> products = new()
        {
            // ... ваши тестовые данные (оставьте как есть для теста)
        };

        public void Add(Product product)
        {
            // ❌ УДАЛИТЕ эту строку: product.ImagePath = "/images/Cyberpunk_2077_Logo.jpg";
            // ✅ Пусть ImagePath остаётся таким, каким его передал контроллер
            products.Add(product);
        }

        public List<Product> GetAll() => products;

        public Product TryGetById(int id) =>
            products.FirstOrDefault(p => p.Id == id);

        public void Update(Product product)
        {
            var existing = products.FirstOrDefault(p => p.Id == product.Id);
            if (existing == null) return;

            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Cost = product.Cost;
            existing.ImagePath = product.ImagePath;
        }

        public void Remove(int id) =>
            products.RemoveAll(p => p.Id == id);
    }
}