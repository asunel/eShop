using eShop.Common.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Bussiness.Interfaces
{
    public interface IOrderManager
    {
        Task CreateOrder(Order order, string cartId, string userId);

        Task<List<OrderDetail>> GetOrderDetails(int orderId);
    }
}
