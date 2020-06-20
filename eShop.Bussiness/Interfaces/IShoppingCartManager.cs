using eShop.Common.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Bussiness.Interfaces
{
    public interface IShoppingCartManager
    {
        Task AddToCart(Product product, int amount);

        Task<int> RemoveFromCart(Product product);

        Task<List<ShoppingCartItem>> GetShoppingCartItems(string cartId, string userId);

        Task ClearCart();

        Task<decimal> GetShoppingCartTotal();
    }
}
