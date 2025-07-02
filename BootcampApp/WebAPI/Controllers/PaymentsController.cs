using BootcampApp.Model;
using BootcampApp.Service;
using Microsoft.AspNetCore.Mvc;
using BootcampApp.Common.BootcampApp.Common.DTOs;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller responsible for handling payment-related operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentsController"/> class.
        /// </summary>
        /// <param name="paymentService">Service for payment processing.</param>
        public PaymentsController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>
        /// Processes and saves a new payment.
        /// </summary>
        /// <param name="dto">Payment data transfer object containing payment details.</param>
        /// <returns>HTTP 200 OK with success message if saved, otherwise appropriate error response.</returns>
        [HttpPost]
        public async Task<IActionResult> PostPayment([FromBody] PaymentDto dto)
        {
            if (dto == null)
                return BadRequest("Payment data is required.");

            try
            {
                var payment = new Payment
                {
                    OrderId = dto.OrderId,
                    PaymentMethodId = dto.PaymentMethodId,
                    Amount = dto.Amount,
                    PaymentDate = DateTime.UtcNow,
                    OrderType = dto.OrderType
                };

                await _paymentService.SavePaymentAsync(payment);

                return Ok(new { message = "Payment saved." });
            }
            catch (Exception ex)
            {
                // Log the detailed error here (Console.WriteLine or a proper logger)
                Console.WriteLine(ex);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
