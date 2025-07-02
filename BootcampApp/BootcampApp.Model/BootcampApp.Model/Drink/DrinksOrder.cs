using System;
using System.Collections.Generic;

namespace BootcampApp.Model
{
    /// <summary>
    /// Represents a drinks order made by a user.
    /// </summary>
    public class DrinksOrder
    {
        /// <summary>
        /// Gets or sets the unique identifier of the order.
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who placed the order.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the order was placed.
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Gets or sets the user who placed the order.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets or sets the list of items included in the drinks order.
        /// </summary>
        public List<DrinkOrderItem> Items { get; set; } = new();

        /// <summary>
        /// Gets or sets the transaction ID for card payment, if applicable. Nullable.
        /// </summary>
        public string? CardPaymentTransactionId { get; set; }
    }
}
