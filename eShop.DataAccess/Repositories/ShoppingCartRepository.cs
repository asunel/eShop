using eShop.Common.DTO;
using eShop.DataAccess.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eShop.DataAccess.Repositories
{
    public class ShoppingCart : IShoppingCartRepository
    {
        private readonly DatabaseContext _appDbContext;

        public ShoppingCart(DatabaseContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public string ShoppingCartId { get; set; }
        public string UserId { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            var httpContextAccessorService = services.GetRequiredService<IHttpContextAccessor>();
            var session = httpContextAccessorService ?. HttpContext.Session;

            var context = services.GetService<DatabaseContext>();
            var cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId, UserId = httpContextAccessorService?.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) };
        }

        public async Task AddToCart(Product product, int amount)
        {
            var shoppingCartItem =
                    await _appDbContext.ShoppingCartItems.SingleOrDefaultAsync(
                        s => s.Product.ProductId == product.ProductId && s.ShoppingCartId == ShoppingCartId && s.UserId == UserId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Product = product,
                    Quantity = 1,
                    UserId =  UserId
            };

                await _appDbContext.ShoppingCartItems.AddAsync(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Quantity++;
            }

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<int> RemoveFromCart(Product product)
        {
            var shoppingCartItem =
                    await _appDbContext.ShoppingCartItems.SingleOrDefaultAsync(
                        s => s.Product.ProductId == product.ProductId && s.ShoppingCartId == ShoppingCartId && s.UserId == UserId);

            var localAmount = 0;

            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.Quantity > 1)
                {
                    shoppingCartItem.Quantity--;
                    localAmount = shoppingCartItem.Quantity;
                }
                else
                {
                    _appDbContext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }

            await _appDbContext.SaveChangesAsync();

            return localAmount;
        }

        public async Task<List<ShoppingCartItem>> GetShoppingCartItems(string cartId, string userId)
        {
            cartId = cartId ?? ShoppingCartId;
            userId = userId ?? UserId;

            return ShoppingCartItems ??
                   (ShoppingCartItems =
                       await _appDbContext.ShoppingCartItems.Where(s => s.ShoppingCartId == cartId && s.UserId == userId)
                           .Include(s => s.Product)
                           .ToListAsync());
        }

        public async Task ClearCart()
        {
            var cartItems = await _appDbContext
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId && cart.UserId == UserId).ToListAsync();

            _appDbContext.ShoppingCartItems.RemoveRange(cartItems);

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<decimal> GetShoppingCartTotal()
        {
            var items = await _appDbContext.ShoppingCartItems.ToListAsync();
            var total = items.Where(c => c.ShoppingCartId == ShoppingCartId && c.UserId == UserId)
                .Select(c => c.Product.Price * c.Quantity).Sum();

            return total;
        }
    }
}
