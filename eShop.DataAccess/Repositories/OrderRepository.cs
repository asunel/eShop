using eShop.Common.DTO;
using eShop.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.DataAccess.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DatabaseContext _appDbContext;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public OrderRepository(DatabaseContext appDbContext, IShoppingCartRepository shoppingCartRepository)
        {
            _appDbContext = appDbContext;
            _shoppingCartRepository = shoppingCartRepository;
        }
        public async Task CreateOrder(Order order, string cartId, string userId)
        {
            order.OrderPlaced = DateTime.Now;
            await _appDbContext.Orders.AddAsync(order);

            var shoppingCartItems = await _shoppingCartRepository.GetShoppingCartItems(cartId, userId);

            foreach (var item in shoppingCartItems)
            {
                var orderDetail = new OrderDetail()
                {
                    Quantity = item.Quantity,
                    ProductId = item.Product.ProductId,
                    OrderId = order.OrderId,
                    Price = item.Product.Price
                };

                await _appDbContext.OrderDetails.AddAsync(orderDetail);
            }

            await _appDbContext.SaveChangesAsync();
        }

        public async Task<List<OrderDetail>> GetOrderDetails(int orderId)
        {
          return await _appDbContext.OrderDetails.Where(o => o.OrderId == orderId).Include(s => s.Product).Include(s=>s.Order).ToListAsync();
        }
    }
}
