using System;

namespace BootcampApp.Common.BootcampApp.Common.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a payment.
    /// </summary>
    public class PaymentDto
    {
        /// <summary>
        /// Gets or sets the ID of the order associated with the payment.
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the payment method used.
        /// </summary>
        public int PaymentMethodId { get; set; }

        /// <summary>
        /// Gets or sets the amount paid.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the type of the order (e.g., "pizza", "drink").
        /// </summary>
        public string OrderType { get; set; }
    }
}
