using System;
using System.Collections.Generic;

namespace BootcampApp.Common.BootcampApp.Common.DTOs
{
    /// <summary>
    /// Represents a request to create a drinks order.
    /// </summary>
    public class CreateDrinksOrderRequest
    {
        /// <summary>
        /// Gets or sets the ID of the user who places the order.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the list of drink items included in the order.
        /// </summary>
        public List<DrinksOrderItemRequest> Items { get; set; }

        /// <summary>
        /// Gets or sets the payment method used for the order (e.g., "Cash", "Card").
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets the transaction ID for card payments, if applicable.
        /// This value can be null if payment is not made by card.
        /// </summary>
        public string? CardPaymentTransactionId { get; set; }
    }

    /// <summary>
    /// Represents a single drink item in a drinks order request.
    /// </summary>
    public class DrinksOrderItemRequest
    {
        /// <summary>
        /// Gets or sets the ID of the drink.
        /// </summary>
        public Guid DrinkId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of this drink to order.
        /// </summary>
        public int Quantity { get; set; }
    }
}
