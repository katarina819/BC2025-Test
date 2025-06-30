using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampApp.Common.BootcampApp.Common.DTOs
{
    public class PaymentDto
    {
        public Guid OrderId { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
        public string OrderType { get; set; }
    }
}
