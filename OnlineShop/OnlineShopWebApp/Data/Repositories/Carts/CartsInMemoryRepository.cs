using OnlineShopWebApp.Data.Models;

namespace OnlineShopWebApp.Data.Repository.Carts
{
    public class CartsInMemoryRepository : ICartsRepository
    {
        private List<Cart> carts = new List<Cart>();
        public Cart TryGetByUserID(string userId)
        {
            return carts.FirstOrDefault(x => x.UserId == userId);
        }

        public void Add(Models.Product product, string userId)
        {
            var existingCart = TryGetByUserID(userId);
            if (existingCart == null)
            {
                var newCart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Items = new List<CartItem>
                    {
                        new CartItem
                        {
                            Id = Guid.NewGuid(),
                            Quantity = 1,
                            Product = product
                        }
                    }
                };
                carts.Add(newCart);
            }
            else
            {
                var existingCartItem = existingCart.Items.FirstOrDefault(x => x.Product.Id == product.Id);
                if (existingCartItem != null)
                {
                    existingCartItem.Quantity ++;
                }
                else
                {
                    existingCart.Items.Add(new CartItem
                    {
                        Id = Guid.NewGuid(),
                        Quantity = 1,
                        Product = product
                    });
                }
            }
        }

        public void DecreaseAmount(int productId, string userId)
        {
            var existingCart = TryGetByUserID(userId);
            var existingCartItem = existingCart?.Items?.FirstOrDefault(x => x.Product.Id == productId);
            if (existingCartItem == null)
            {
                return;
            }
            existingCartItem.Quantity --;

            if (existingCartItem.Quantity == 0)
            {
                existingCart.Items.Remove(existingCartItem);
            }


        }

        public void Clear(string userId)
        {
            var existingCart = TryGetByUserID(userId);
            carts.Remove(existingCart);
        }
    }
}