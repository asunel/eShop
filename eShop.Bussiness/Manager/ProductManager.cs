using eShop.Bussiness.Interfaces;
using eShop.Common.DTO;
using eShop.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.Bussiness.Manager
{
    public class ProductManager : IProductManager
    {
        private readonly IProductRepository _productRepository;

        public ProductManager(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        
        public async Task<Product> GetProduct(int? productId)
        {
            return await _productRepository.GetProduct(productId);
        }

        public async Task<IEnumerable<Product>> GetProducts(string searchString)
        {
            return await _productRepository.GetProducts(searchString);
        }

        public async Task<IEnumerable<Product>> GetProducts() {
            return await _productRepository.GetProducts();
        }

        public async Task<int> AddProduct(Product product) {
            return await _productRepository.AddProduct(product);
        }

        public async Task<Product> UpdateProduct(Product product) {
            return await _productRepository.UpdateProduct(product);
        }

        public async Task<bool> DeleteProduct(int? productId)
        {
            return await _productRepository.DeleteProduct(productId);
        }

        public async Task<bool> UpdateProductPrice(int productId, Product product)
        {
            return await _productRepository.UpdateProductPrice(productId, product);
        }
    }
}
