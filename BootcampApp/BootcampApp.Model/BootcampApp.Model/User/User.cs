using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace BootcampApp.Model
{
    /// <summary>
    /// Represents a user entity with basic details and profile.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the age of the user. Nullable.
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// Gets or sets the user's profile.
        /// </summary>
        public UserProfile Profile { get; set; }
    }

    /// <summary>
    /// Represents a user's profile details.
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// Gets or sets the associated user's identifier. Nullable.
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// Gets or sets the user's phone number. Nullable.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the user's address. Nullable.
        /// </summary>
        public string? Address { get; set; }

        /// <summary>
        /// Gets or sets the user linked to this profile.
        /// This property is ignored during JSON serialization to prevent circular references.
        /// </summary>
        [JsonIgnore]
        public User User { get; set; }
    }

    /// <summary>
    /// Represents a pizza item with details like name, size, price, and dietary info.
    /// </summary>
    public class PizzaItem
    {
        /// <summary>
        /// Gets or sets the unique identifier of the pizza.
        /// </summary>
        public Guid PizzaId { get; set; }

        /// <summary>
        /// Gets or sets the name of the pizza.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the size of the pizza (e.g., small, medium, large).
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// Gets or sets the price of the pizza.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the pizza is vegetarian.
        /// </summary>
        public bool IsVegetarian { get; set; }
    }

    /// <summary>
    /// Represents a single item in a pizza order.
    /// </summary>
    public class PizzaOrderItem
    {
        /// <summary>
        /// Gets or sets the unique identifier of the pizza order item.
        /// </summary>
        public Guid OrderItemId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated pizza order.
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the pizza.
        /// </summary>
        public Guid PizzaId { get; set; }

        /// <summary>
        /// Gets or sets the pizza details for this order item.
        /// </summary>
        public PizzaItem Pizza { get; set; }

        /// <summary>
        /// Gets or sets the quantity of this pizza ordered.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price at the time of order.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the total price for this order item.
        /// </summary>
        public decimal TotalPrice { get; set; }
    }

    /// <summary>
    /// Represents a pizza order made by a user.
    /// </summary>
    public class PizzaOrder
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
        /// Gets or sets the list of pizza order items.
        /// </summary>
        public List<PizzaOrderItem> Items { get; set; } = new();

        /// <summary>
        /// Checks whether the user's email is valid by verifying it contains '@'.
        /// </summary>
        /// <returns>True if email is valid; otherwise false.</returns>
        public bool IsEmailValid()
        {
            return User?.Email?.Contains("@") == true;
        }
    }
}
