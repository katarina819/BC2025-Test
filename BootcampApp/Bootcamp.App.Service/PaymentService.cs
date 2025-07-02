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

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentService"/> class.
        /// </summary>
        /// <param name="paymentRepo">The payment repository used for data access.</param>
        public PaymentService(PaymentRepository paymentRepo)
        {
            _paymentRepo = paymentRepo;
        }

        /// <summary>
        /// Saves a payment asynchronously after verifying the corresponding order exists.
        /// </summary>
        /// <param name="payment">The payment entity to save.</param>
        /// <exception cref="Exception">Thrown when the related order does not exist.</exception>
        public async Task SavePaymentAsync(Payment payment)
        {
            var exists = await _paymentRepo.OrderExistsAsync(payment.OrderId, payment.OrderType);
            if (!exists)
                throw new Exception($"Order with ID {payment.OrderId} of type '{payment.OrderType}' does not exist.");

            await _paymentRepo.AddPaymentAsync(payment);
        }
    }
}
