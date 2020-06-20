using System;
using System.ComponentModel.DataAnnotations;

namespace eShop.Common.DTO
{
    public class Order
    {
        public int OrderId { get; set; }

        [ScaffoldColumn(false)]
        public DateTime OrderPlaced { get; set; }
    }
}
