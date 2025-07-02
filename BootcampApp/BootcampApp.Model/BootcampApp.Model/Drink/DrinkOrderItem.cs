using System;

namespace BootcampApp.Model
{
    /// <summary>
    /// Represents a single item in a drink order.
    /// </summary>
    public class DrinkOrderItem
    {
        /// <summary>
        /// Gets or sets the unique identifier of the order item.
        /// </summary>
        public Guid OrderItemId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated order.
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the drink.
        /// </summary>
        public Guid DrinkId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the drink ordered.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the drink at the time of order.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets the total cost for this item (Quantity multiplied by UnitPrice).
        /// </summary>
        public decimal TotalCost => Quantity * UnitPrice;

        /// <summary>
        /// Gets or sets the Drink entity related to this order item.
        /// </summary>
        public Drink Drink { get; set; }
    }
}
