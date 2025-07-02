namespace WebAPI.REST
{
    /// <summary>
    /// Represents a user data transfer object for REST responses.
    /// </summary>
    public class UserREST
    {
        /// <summary>
        /// Gets or sets the unique identifier of the user.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the user's name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the user's email address.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the user's age. Nullable.
        /// </summary>
        public int? Age { get; set; }

        /// <summary>
        /// Gets or sets the user's profile information.
        /// </summary>
        public required UserProfileREST Profile { get; set; }
    }

    /// <summary>
    /// Represents a user's profile information for REST responses.
    /// </summary>
    public class UserProfileREST
    {
        /// <summary>
        /// Gets or sets the user's phone number.
        /// </summary>
        public required string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the user's address.
        /// </summary>
        public required string Address { get; set; }
    }

    /// <summary>
    /// Represents a pizza item for REST responses.
    /// </summary>
    public class PizzaItemREST
    {
        /// <summary>
        /// Gets or sets the unique identifier of the pizza.
        /// </summary>
        public Guid PizzaId { get; set; }

        /// <summary>
        /// Gets or sets the pizza name.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Gets or sets the pizza size (e.g., small, medium, large).
        /// </summary>
        public required string Size { get; set; }

        /// <summary>
        /// Gets or sets the pizza price.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Gets or sets whether the pizza is vegetarian.
        /// </summary>
        public bool IsVegetarian { get; set; }
    }

    /// <summary>
    /// Represents an item in a pizza order for REST responses.
    /// </summary>
    public class PizzaOrderItemREST
    {
        /// <summary>
        /// Gets or sets the unique identifier of the order item.
        /// </summary>
        public Guid OrderItemId { get; set; }

        /// <summary>
        /// Gets or sets the pizza associated with this order item.
        /// </summary>
        public required PizzaItemREST Pizza { get; set; }

        /// <summary>
        /// Gets or sets the quantity ordered.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the price per unit of the pizza.
        /// </summary>
        public decimal UnitPrice { get; set; }
    }

    /// <summary>
    /// Represents a pizza order with user and items information for REST responses.
    /// </summary>
    public class PizzaOrderREST
    {
        /// <summary>
        /// Gets or sets the unique identifier of the order.
        /// </summary>
        public Guid OrderId { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the order was placed.
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Gets or sets the user who placed the order.
        /// </summary>
        public required UserREST User { get; set; }

        /// <summary>
        /// Gets or sets the list of items included in the order.
        /// </summary>
        public required List<PizzaOrderItemREST> Items { get; set; }
    }
}
