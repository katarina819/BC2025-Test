using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;
using BootcampApp.Service;
using Microsoft.AspNetCore.Mvc;

namespace BootcampApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrinksOrderController : ControllerBase
    {
        private readonly IDrinksOrderService _drinksOrderService;

        public DrinksOrderController(IDrinksOrderService drinksOrderService)
        {
            _drinksOrderService = drinksOrderService;
        }

        // GET: api/DrinksOrder
        [HttpGet]
        public ActionResult<IEnumerable<DrinksOrder>> GetAllOrders()
        {
            var orders = _drinksOrderService.GetDrinksOrdersWithDetails();
            return Ok(orders);
        }

        // GET: api/DrinksOrder/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DrinksOrder>> GetOrderById(Guid id)
        {
            var order = await _drinksOrderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            return Ok(order);
        }

        // POST: api/DrinksOrder
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateOrder([FromBody] DrinksOrder order)
        {
            var newOrderId = await _drinksOrderService.CreateOrderAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = newOrderId }, newOrderId);
        }

        // DELETE: api/DrinksOrder/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(Guid id)
        {
            var deleted = await _drinksOrderService.DeleteOrderAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}

