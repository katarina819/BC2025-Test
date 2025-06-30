using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootcampApp.Model
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public Guid OrderId { get; set; }
        public int PaymentMethodId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string OrderType { get; set; }  // "pizza" ili "drink"
    }
}
