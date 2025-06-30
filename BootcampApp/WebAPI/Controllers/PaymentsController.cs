using BootcampApp.Model;
using BootcampApp.Service;
using Microsoft.AspNetCore.Mvc;
using BootcampApp.Common.BootcampApp.Common.DTOs;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly PaymentService _paymentService;

        public PaymentsController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

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
                // Zapiši točno što je pošlo po zlu
                Console.WriteLine(ex); // ili _logger.LogError
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
