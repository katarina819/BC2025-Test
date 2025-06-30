using System;
using System.Text.Json.Serialization;

namespace BootcampApp.Model
{
    public class User
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; } = string.Empty;     // 👈 DODAJ
        public string Email { get; set; } = string.Empty;
        public int? Age { get; set; }
        
        public UserProfile Profile { get; set; }
    }

    public class UserProfile
    {
        public Guid? UserId { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }



        [JsonIgnore] 
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
        public Guid PizzaId { get; set; }  
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

        
        public bool IsEmailValid()
        {
            
            return User?.Email?.Contains("@") == true;
        }

    }

        

        

        

}


