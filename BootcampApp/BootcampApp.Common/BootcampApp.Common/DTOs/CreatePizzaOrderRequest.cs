using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampApp.Common.BootcampApp.Common.DTOs
{
    public class CreatePizzaOrderRequest
    {
        public Guid UserId { get; set; }
        public List<CreatePizzaOrderItemRequest> Items { get; set; }
        public string PaymentMethod { get; set; }

    }

    public class CreatePizzaOrderItemRequest
    {
        public Guid PizzaId { get; set; }
        public int Quantity { get; set; }
    }
}
