using Microsoft.EntityFrameworkCore;
using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data.Repository.Carts
{
    public class CartsEfRepository : ICartsRepository
    {
        private readonly AppDbContext _context;
        public CartsEfRepository(AppDbContext context) => _context = context;

        public Cart TryGetByUserID(string userId) =>
            _context.Carts.Include(c => c.Items).ThenInclude(i => i.Product)
                          .AsNoTracking()
                          .FirstOrDefault(c => c.UserId == userId);

        public void Add(Models.Product product, string userId)
        {
            var cart = TryGetByUserID(userId);
            if (cart == null)
            {
                cart = new Cart { Id = Guid.NewGuid(), UserId = userId };
                _context.Carts.Add(cart);
            }

            var item = cart.Items.FirstOrDefault(i => i.ProductId == product.Id);
            if (item != null) item.Quantity++;
            else cart.Items.Add(new CartItem { Id = Guid.NewGuid(), ProductId = product.Id, Quantity = 1 });

            _context.SaveChanges();
        }

        public void DecreaseAmount(int productId, string userId)
        {
            var cart = _context.Carts.Include(c => c.Items).FirstOrDefault(c => c.UserId == userId);
            if (cart == null) return;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) return;

            item.Quantity--;
            if (item.Quantity <= 0) cart.Items.Remove(item);
            _context.SaveChanges();
        }

        public void Clear(string userId)
        {
            var cart = _context.Carts.Include(c => c.Items).FirstOrDefault(c => c.UserId == userId);
            if (cart == null) return;

            _context.CartItems.RemoveRange(cart.Items);
            _context.Carts.Remove(cart);
            _context.SaveChanges();
        }
    }
}