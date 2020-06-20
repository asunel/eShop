using eShop.Common.DTO;
using System.Collections.Generic;

namespace eShop.ViewModels
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
    }
}
