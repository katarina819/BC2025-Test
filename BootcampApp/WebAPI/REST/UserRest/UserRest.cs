namespace WebAPI.REST
{
    public class UserREST
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public int? Age { get; set; }
        public required UserProfileREST Profile { get; set; }
    }

    public class UserProfileREST
    {
        public required string PhoneNumber { get; set; }
        public required string Address { get; set; }
    }

    public class PizzaItemREST
    {
        public Guid PizzaId { get; set; }
        public required string Name { get; set; }
        public required string Size { get; set; }
        public decimal Price { get; set; }
        public bool IsVegetarian { get; set; }
    }

    public class PizzaOrderItemREST
    {
        public Guid OrderItemId { get; set; }
        public required PizzaItemREST Pizza { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class PizzaOrderREST
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public required UserREST User { get; set; }
        public required List<PizzaOrderItemREST> Items { get; set; }
    }
}
