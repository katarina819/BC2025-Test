using System;
using System.Collections.Generic;

namespace BootcampApp.Model
{
    public class DrinksOrder
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }

        public User User { get; set; }
        public List<DrinkOrderItem> Items { get; set; } = new();

        public string? CardPaymentTransactionId { get; set; }
    }
}
