using System;

namespace BootcampApp.Model
{
    /// <summary>
    /// Represents a drink entity with details such as name, size, and price.
    /// </summary>
    public class Drink
    {
        /// <summary>
        /// Gets or sets the unique identifier of the drink.
        /// </summary>
        public Guid DrinkId { get; set; }

        /// <summary>
        /// Gets or sets the name of the drink.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the size of the drink (e.g., small, medium, large). Nullable.
        /// </summary>
        public string? Size { get; set; }

        /// <summary>
        /// Gets or sets the price of the drink.
        /// </summary>
        public decimal Price { get; set; }
    }
}
