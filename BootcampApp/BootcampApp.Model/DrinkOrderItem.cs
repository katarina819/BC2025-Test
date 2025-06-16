using System;

namespace BootcampApp.Model
{
    public class DrinkOrderItem
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Guid DrinkId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalCost => Quantity * UnitPrice;


        public Drink Drink { get; set; }
    }
}
