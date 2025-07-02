using System;

namespace BootcampApp.Model
{
    /// <summary>
    /// Represents a payment made for an order.
    /// </summary>
    public class Payment
    {
        /// <summary>
        /// Gets or sets the unique identifier of the payment.
        /// </summary>
        public int PaymentId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the order associated with the payment.
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the payment method used.
        /// </summary>
        public int PaymentMethodId { get; set; }

        /// <summary>
        /// Gets or sets the total amount paid.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the payment was made.
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the order (e.g., "pizza" or "drink").
        /// </summary>
        public string OrderType { get; set; }
    }
}
