﻿using eShop.Bussiness.Interfaces;
using eShop.DataAccess.Repositories;
using eShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eShop.Controllers
{
    public class ShoppingCartController: Controller
    {
        private readonly IProductManager _productManager;

        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartController(IProductManager productManager, ShoppingCart shoppingCart)
        {
            _productManager = productManager;
            _shoppingCart = shoppingCart;
        }

        public async Task<ViewResult> Index()
        {
            var items = await _shoppingCart.GetShoppingCartItems(_shoppingCart.ShoppingCartId, _shoppingCart.UserId);
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = await _shoppingCart.GetShoppingCartTotal()
            };
            return View(shoppingCartViewModel);
        }

        public async Task<RedirectToActionResult> AddToShoppingCart(int productId)
        {
            var selectedProduct = await _productManager.GetProduct(productId);
            if (selectedProduct != null)
            {
                await _shoppingCart.AddToCart(selectedProduct, 1);
            }
            return RedirectToAction("List", "Product");
        }

        public  async Task<RedirectToActionResult> RemoveFromShoppingCart(int productId)
        {
            var selectedProduct = await _productManager.GetProduct(productId);
            if (selectedProduct != null)
            {
                await _shoppingCart.RemoveFromCart(selectedProduct);
            }
            return RedirectToAction("Index");
        }
    }
}
