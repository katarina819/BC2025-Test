using System;

namespace BootcampApp.Model
{
    public class Drink
    {
        public Guid DrinkId { get; set; }
        public string Name { get; set; }
        public string? Size { get; set; }
        public decimal Price { get; set; }
    }
}
