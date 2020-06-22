using eShop.Business.Interfaces;
using eShop.Common.DTO;
using eShop.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Business.Manager
{
    public class ShoppingCartManager : IShoppingCartManager
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public ShoppingCartManager(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public async Task AddToCart(Product product, int amount)
        {
            await _shoppingCartRepository.AddToCart(product, amount);
        }

        public async Task<int> RemoveFromCart(Product product)
        {
            return await _shoppingCartRepository.RemoveFromCart(product);
        }

        public async Task<List<ShoppingCartItem>> GetShoppingCartItems(string cartId, string userId)
        {
            return await _shoppingCartRepository.GetShoppingCartItems(cartId, userId);
        }

        public async Task ClearCart()
        {
            await _shoppingCartRepository.ClearCart();
        }

        public Task<decimal> GetShoppingCartTotal()
        {
            return _shoppingCartRepository.GetShoppingCartTotal();
        }
    }
}
