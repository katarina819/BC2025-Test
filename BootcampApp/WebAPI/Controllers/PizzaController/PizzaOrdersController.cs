using System.Linq;
using BootcampApp.Common.BootcampApp.Common.DTOs;
using BootcampApp.Model;
using BootcampApp.Service;
using Microsoft.AspNetCore.Mvc;
using WebAPI.REST;
using BootcampApp.Model.Entities.Pizza;
using BootcampApp.Service.BootcampApp.Service.PizzaService;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for handling pizza orders.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PizzaOrdersController : ControllerBase
    {
        private readonly IPizzaService _pizzaService;
        private readonly IPizzaOrderService _pizzaOrderService;

        /// <summary>
        /// Initializes a new instance of <see cref="PizzaOrdersController"/>.
        /// </summary>
        /// <param name="pizzaService">Service to manage pizzas.</param>
        /// <param name="pizzaOrderService">Service to manage pizza orders.</param>
        public PizzaOrdersController(IPizzaService pizzaService, IPizzaOrderService pizzaOrderService)
        {
            _pizzaService = pizzaService;
            _pizzaOrderService = pizzaOrderService;
        }

        /*
        // Uncomment and document these methods if needed:

        /// <summary>
        /// Gets all pizza orders with details.
        /// </summary>
        /// <returns>List of pizza orders including items and user info.</returns>
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            // Implementation...
        }

        /// <summary>
        /// Gets a pizza order by its ID.
        /// </summary>
        /// <param name="id">Order ID.</param>
        /// <returns>Pizza order details or NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            // Implementation...
        }

        /// <summary>
        /// Deletes a pizza order by its ID.
        /// </summary>
        /// <param name="id">Order ID.</param>
        /// <returns>NoContent if deleted, NotFound otherwise.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            // Implementation...
        }
        */

        /// <summary>
        /// Retrieves all available pizzas.
        /// </summary>
        /// <returns>List of pizzas.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PizzaItem>>> GetAll()
        {
            var pizzas = await _pizzaService.GetAllAsync();
            return Ok(pizzas);
        }

        /// <summary>
        /// Creates a new pizza order.
        /// </summary>
        /// <param name="request">The pizza order creation request containing the items.</param>
        /// <returns>The created order's ID if successful, otherwise a BadRequest response.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePizzaOrderRequest request)
        {
            if (request == null || request.Items == null || !request.Items.Any())
                return BadRequest("Order must contain at least one item.");

            if (request.Items.Any(i => i.Quantity <= 0))
                return BadRequest("Each item must have a quantity greater than zero.");

            var createdOrder = await _pizzaOrderService.CreateOrderAsync(request);

            // Return the order ID in the response:
            return Ok(new { orderId = createdOrder.OrderId });
        }
    }
}
