using eShop.Bussiness.Interfaces;
using eShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;


namespace eShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductManager _productManager;

        public ProductController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public async Task<ViewResult> List()
        {
            var products = await _productManager.GetProducts();
            products = products.OrderBy(p => p.ProductId);

            var vm = new ProductListViewModel()
            {
                Products = products,
            };

            return View(vm);
        }

        public async Task<ViewResult> Search(string searchString)
        {
            var products = await _productManager.GetProducts(searchString);
            products = products.OrderBy(p => p.ProductId);

            var vm = new ProductListViewModel()
            {
                Products = products,
            };

            return View(vm);
        }
    }
}
