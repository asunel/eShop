using eShop.Common.DTO;
using eShop.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eShop.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseContext _ctx;
        public ProductRepository(DatabaseContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Product> GetProduct(int? productId)
        {
            return await _ctx.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _ctx.Products.ToListAsync();
        }

        public async Task<int> AddProduct(Product product)
        {
            await _ctx.Products.AddAsync(product);
            await _ctx.SaveChangesAsync();
            return product.ProductId;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            var existingProduct = await _ctx.Products
           .Where(s => s.ProductId == product.ProductId)
           .FirstOrDefaultAsync();

            if (existingProduct != null)
            {
                _ctx.Entry(existingProduct).State = EntityState.Detached;

                _ctx.Products.Update(product);
                await _ctx.SaveChangesAsync();
                return product;
            }

            return null;
        }

        public async Task<bool> UpdateProductPrice(int productId, Product product)
        {
            var existingProduct = await _ctx.Products.SingleOrDefaultAsync(p => p.ProductId == productId);

            if (existingProduct != null)
            {
                existingProduct.Price = product.Price;
                var changes = await _ctx.SaveChangesAsync();
                if (changes > 0)
                {
                    return true;
                }

                return false;
            }

            return false;
        }
        public async Task<bool> DeleteProduct(int? productId)
        {
            var product = await _ctx.Products
           .Where(s => s.ProductId == productId)
           .FirstOrDefaultAsync();

            _ctx.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            var changes = await _ctx.SaveChangesAsync();

            if (changes >= 0)
            {
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Product>> GetProducts(string searchString)
        {
            var products = await _ctx.Products.ToListAsync();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                return products.Where(p => p.Name.Contains(searchString, System.StringComparison.OrdinalIgnoreCase));
            }

            return products;
        }
    }
}
