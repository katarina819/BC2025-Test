using System;
using System.Collections.Generic;

namespace BootcampApp.Common.BootcampApp.Common.DTOs
{
    /// <summary>
    /// Data Transfer Object (DTO) representing a drink order.
    /// </summary>
    public class DrinkOrderDto
    {
        /// <summary>
        /// Gets or sets the ID of the user who placed the drink order.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the list of drink order items included in this order.
        /// </summary>
        public List<DrinkOrderItemDto> Items { get; set; } = new();
    }
}
