using System;

namespace BootcampApp.Model
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int? Age { get; set; }
        public UserProfile Profile { get; set; }
    }

    public class UserProfile
    {
        public Guid UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public User User { get; set; }
    }

    public class PizzaItem
    {
        public Guid PizzaId { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public bool IsVegetarian { get; set; }
    }

    public class PizzaOrderItem
    {
        public Guid OrderItemId { get; set; }
        public Guid OrderId { get; set; }
        public Guid PizzaId { get; set; }  // Ovdje dodaješ taj property
        public PizzaItem Pizza { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class PizzaOrder
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public User User { get; set; }
        public List<PizzaOrderItem> Items { get; set; } = new();

        // Ovdje je greška jer nemate Email property u PizzaOrder, a pozivate ga u metodi
        public bool IsEmailValid()
        {
            // Ako želite provjeriti korisnikov email:
            return User?.Email?.Contains("@") == true;
        }

    }

        

        

        

}


