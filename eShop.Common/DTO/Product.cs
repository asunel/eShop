using System;

namespace eShop.Common.DTO
{
    public class Product
    {
        public Int32 ProductId { get; set; }  //recognized by Entity framework as identifier for the class

        public string Name { get; set; }

        public string Description { get; set; } // description shown below small image

        public string Image { get; set; } // for the details view

        public decimal Price { get; set; } // price
    }
}
