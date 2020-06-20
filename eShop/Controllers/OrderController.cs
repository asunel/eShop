using System.Security.Claims;
using eShop.Bussiness.Interfaces;
using eShop.Common.DTO;
using eShop.DataAccess.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderManager _orderManager;

        private readonly ShoppingCart _shoppingCart;

        public OrderController(IOrderManager orderManager, ShoppingCart shoppingCart)
        {
            _orderManager = orderManager;
            _shoppingCart = shoppingCart;
        }

        [Authorize]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cartId = _shoppingCart.ShoppingCartId;
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var items = await _shoppingCart.GetShoppingCartItems(cartId, userId);
            _shoppingCart.ShoppingCartItems = items;

            if (_shoppingCart.ShoppingCartItems.Count <= 0)
            {
                ModelState.AddModelError("", "Your cart is empty, add some products first");
            }

            if (ModelState.IsValid)
            {
                await _orderManager.CreateOrder(order, cartId, userId);
                await _shoppingCart.ClearCart();
            }

            ViewBag.CheckoutCompleteMessage = "Order placed successfully! :)";
            return View(order);
        }

        public async Task<IActionResult> Index(int orderId)
        {
            var orderDetails = await _orderManager.GetOrderDetails(orderId);
            return View(orderDetails);
        }
    }
}
