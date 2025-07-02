using System;

namespace BootcampApp.Model
{
    /// <summary>
    /// Represents a pizza with prices for different sizes.
    /// </summary>
    public class Pizza
    {
        /// <summary>
        /// Gets or sets the unique identifier of the pizza.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the pizza.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the price for a small size pizza.
        /// </summary>
        public decimal PriceSmall { get; set; }

        /// <summary>
        /// Gets or sets the price for a medium size pizza.
        /// </summary>
        public decimal PriceMedium { get; set; }

        /// <summary>
        /// Gets or sets the price for a large size pizza.
        /// </summary>
        public decimal PriceLarge { get; set; }
    }
}
