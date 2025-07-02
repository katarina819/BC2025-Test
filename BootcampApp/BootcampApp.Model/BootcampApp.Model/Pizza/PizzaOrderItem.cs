using System;

namespace BootcampApp.Model.Entities.Pizza
{
    /// <summary>
    /// Represents a single item in a pizza order.
    /// </summary>
    public class PizzaOrderItem
    {
        /// <summary>
        /// Gets or sets the unique identifier of the order item.
        /// Defaults to a new GUID.
        /// </summary>
        public Guid OrderItemId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the identifier of the associated pizza order.
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the pizza.
        /// </summary>
        public Guid PizzaId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of pizzas ordered.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price of a single pizza at the time of order.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets the total price for this order item (UnitPrice multiplied by Quantity).
        /// </summary>
        public decimal TotalPrice => UnitPrice * Quantity;
    }
}
