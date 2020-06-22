using eShop.Business.Interfaces;
using eShop.Common.DTO;
using eShop.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Business.Manager
{
    public class OrderManager: IOrderManager
    {
        private readonly IOrderRepository _orderRepository;

        public OrderManager(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task CreateOrder(Order order, string cartId, string userId)
        {
            await _orderRepository.CreateOrder(order, cartId, userId);
        }

        public async Task<List<OrderDetail>> GetOrderDetails(int orderId)
        {
            return await _orderRepository.GetOrderDetails(orderId);
        }
    }
}
