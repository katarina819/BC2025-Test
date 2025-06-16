namespace WebAPI.REST
{
    public class UserREST
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? Age { get; set; }
        public UserProfileREST Profile { get; set; }
    }

    public class UserProfileREST
    {
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }

    public class PizzaItemREST
    {
        public Guid PizzaId { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public bool IsVegetarian { get; set; }
    }

    public class PizzaOrderItemREST
    {
        public Guid OrderItemId { get; set; }
        public PizzaItemREST Pizza { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class PizzaOrderREST
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public UserREST User { get; set; }
        public List<PizzaOrderItemREST> Items { get; set; }
    }

}
