using System;

namespace BootcampApp.Common.BootcampApp.Common.DTOs
{
    /// <summary>
    /// Represents a request to create a payment for an order.
    /// </summary>
    public class CreatePaymentRequest
    {
        /// <summary>
        /// Gets or sets the ID of the order to be paid.
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the payment method used.
        /// </summary>
        public int PaymentMethodId { get; set; }

        /// <summary>
        /// Gets or sets the amount of the payment.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the type of the order (e.g., "pizza", "drink").
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the payment was made.
        /// Nullable if the payment date is not set.
        /// </summary>
        public DateTime? PaymentDate { get; set; }
    }
}
