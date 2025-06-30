using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampApp.Common.BootcampApp.Common.DTOs
{
    public class CreateDrinksOrderRequest
    {
        public Guid UserId { get; set; }
        public List<DrinksOrderItemRequest> Items { get; set; }
        public string PaymentMethod { get; set; }
        public string? CardPaymentTransactionId { get; set; }

    }

    public class DrinksOrderItemRequest
    {
        public Guid DrinkId { get; set; }
        public int Quantity { get; set; }
    }
}
