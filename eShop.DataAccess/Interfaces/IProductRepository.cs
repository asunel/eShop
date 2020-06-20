using eShop.Common.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop.DataAccess.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProduct(int? productId);

        Task<IEnumerable<Product>> GetProducts();

        Task<int> AddProduct(Product product);

        Task<Product> UpdateProduct(Product product);

        Task<bool> DeleteProduct(int? productId);

        Task<bool> UpdateProductPrice(int productId, Product product);
        Task<IEnumerable<Product>> GetProducts(string searchString);
    }
}
