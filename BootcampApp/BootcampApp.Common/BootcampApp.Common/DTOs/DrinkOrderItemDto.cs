using System;

namespace BootcampApp.Common.BootcampApp.Common.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a single item in a drink order.
    /// </summary>
    public class DrinkOrderItemDto
    {
        /// <summary>
        /// Gets or sets the ID of the drink.
        /// </summary>
        public Guid DrinkId { get; set; }

        /// <summary>
        /// Gets or sets the quantity of the drink ordered.
        /// </summary>
        public int Quantity { get; set; }
    }
}
