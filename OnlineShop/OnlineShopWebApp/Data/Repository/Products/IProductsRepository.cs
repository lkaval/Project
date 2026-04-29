using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data.Repository.Products
{
    public interface IProductsRepository
    {
        List<Product> GetAll();
        Product TryGetById(int id);
        void Add(Product product);
        void Update(Product product);
    }
}