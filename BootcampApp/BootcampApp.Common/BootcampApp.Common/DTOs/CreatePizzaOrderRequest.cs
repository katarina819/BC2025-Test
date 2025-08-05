using System;
using System.Collections.Generic;

namespace BootcampApp.Common.BootcampApp.Common.DTOs
{
    /// <summary>
    /// Represents a request to create a pizza order.
    /// </summary>
    public class CreatePizzaOrderRequest
    {
        /// <summary>
        /// Gets or sets the ID of the user placing the order.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the list of pizza items included in the order.
        /// </summary>
        public List<CreatePizzaOrderItemRequest> Items { get; set; } = new();

        /// <summary>
        /// Gets or sets the payment method used for the order (e.g., "Cash", "Card").
        /// </summary>
        public string PaymentMethod { get; set; } = string.Empty;
    }

    /// <summary>
    /// Represents a single pizza item in a pizza order request.
    /// </summary>
    public class CreatePizzaOrderItemRequest
    {
        /// <summary>
        /// Gets or sets the ID of the pizza.
        /// </summary>
        public Guid PizzaId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the pizza to order.
        /// </summary>
        public int Quantity { get; set; }
    }
}
