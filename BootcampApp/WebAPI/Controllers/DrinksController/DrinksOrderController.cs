using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BootcampApp.Common.BootcampApp.Common.DTOs;
using BootcampApp.Model;
using BootcampApp.SignalR.Hubs;
using BootcampApp.Repository;
using BootcampApp.Service;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Mvc;

namespace BootcampApp.Controllers
{
    /// <summary>
    /// API controller for managing drink orders.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DrinksOrderController : ControllerBase
    {
        private readonly IDrinksOrderService _drinksOrderService;
        private readonly IHubContext<NotificationHub> _hubContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinksOrderController"/> class.
        /// </summary>
        /// <param name="drinksOrderService">Service for managing drinks orders.</param>
        public DrinksOrderController(IDrinksOrderService drinksOrderService, IHubContext<NotificationHub> hubContext)
        {
            _drinksOrderService = drinksOrderService;
            _hubContext = hubContext;
        }

        /*
        /// <summary>
        /// Retrieves all drinks orders with details.
        /// </summary>
        /// <returns>A list of all drinks orders including user and items details.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _drinksOrderService.GetAllAsync();

            var result = orders.Select(o => new
            {
                o.OrderId,
                o.OrderDate,
                user = new
                {
                    o.User.Id,
                    o.User.Name,
                    o.User.Email,
                    profile = new
                    {
                        o.User.Profile.PhoneNumber,
                        o.User.Profile.Address
                    }
                },
                items = o.Items.Select(i => new
                {
                    i.Quantity,
                    i.UnitPrice,
                    i.TotalCost,
                    drink = new
                    {
                        i.Drink.DrinkId,
                        i.Drink.Name,
                        i.Drink.Size,
                        i.Drink.Price
                    }
                })
            });

            return Ok(result);
        }


        /// <summary>
        /// Retrieves a drinks order by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the drinks order.</param>
        /// <returns>The drinks order if found; otherwise, NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<DrinksOrder>> GetOrderById(Guid id)
        {
            var order = await _drinksOrderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }
        */

        /// <summary>
        /// Retrieves all drinks.
        /// </summary>
        /// <returns>A list of all drinks.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drink>>> GetAll()
        {
            var drinks = await _drinksOrderService.GetAllAsync();
            return Ok(drinks);
        }

        /// <summary>
        /// Creates a new drinks order.
        /// </summary>
        /// <param name="request">The details of the drinks order to create.</param>
        /// <returns>The created drinks order.</returns>
        /// <response code="400">If the order is invalid or contains no items.</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDrinksOrderRequest request)
        {
            if (request == null || request.Items == null || !request.Items.Any())
                return BadRequest("Order must contain at least one item.");

            if (request.Items.Any(i => i.Quantity <= 0))
                return BadRequest("Each drink must have a quantity greater than zero.");

            var createdOrder = await _drinksOrderService.CreateOrderAsync(request);

            string userId = request.UserId.ToString();
            await _hubContext.Clients.User(userId).SendAsync("ReceiveNotification", "A new drinks order has been received!");
            return Ok(createdOrder);
        }

        /*
        /// <summary>
        /// Deletes a drinks order by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the drinks order to delete.</param>
        /// <returns>NoContent if deleted; otherwise, NotFound.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(Guid id)
        {
            var deleted = await _drinksOrderService.DeleteOrderAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
        */
    }
}
