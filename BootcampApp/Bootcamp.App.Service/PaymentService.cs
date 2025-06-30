using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BootcampApp.Model;
using BootcampApp.Repository;


namespace BootcampApp.Service
{
    public class PaymentService
    {
        private readonly PaymentRepository _paymentRepo;

        public PaymentService(PaymentRepository paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }


        public async Task SavePaymentAsync(Payment payment)
        {
            var exists = await _paymentRepo.OrderExistsAsync(payment.OrderId, payment.OrderType);
            if (!exists)
                throw new Exception($"Order with ID {payment.OrderId} of type '{payment.OrderType}' does not exist.");

            await _paymentRepo.AddPaymentAsync(payment);
        }

        
    }

}
