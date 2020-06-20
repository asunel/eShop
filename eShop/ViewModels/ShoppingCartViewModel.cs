using eShop.DataAccess.Repositories;

namespace eShop.ViewModels
{
    public class ShoppingCartViewModel
    {
        public ShoppingCart ShoppingCart { get; set; }

        public decimal ShoppingCartTotal { get; set;  }
    }
}
