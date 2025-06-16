using System.Linq;
using BootcampApp.Model;
using BootcampApp.Service;
using Microsoft.AspNetCore.Mvc;
using WebAPI.REST;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PizzaOrdersController : ControllerBase
    {
        private readonly IPizzaOrderService _pizzaOrderService;

        // Konstruktor prima servis preko DI
        public PizzaOrdersController(IPizzaOrderService pizzaOrderService)
        {
            _pizzaOrderService = pizzaOrderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _pizzaOrderService.GetAllOrdersWithDetailsAsync();


            var ordersREST = orders.Select(o => new PizzaOrderREST
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                User = new UserREST
                {
                    Id = o.User.Id,
                    Name = o.User.Name,
                    Email = o.User.Email,
                    Age = o.User.Age,
                    Profile = o.User.Profile == null ? null : new UserProfileREST
                    {
                        PhoneNumber = o.User.Profile.PhoneNumber,
                        Address = o.User.Profile.Address
                    }
                },
                Items = o.Items.Select(i => new PizzaOrderItemREST
                {
                    OrderItemId = i.OrderItemId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Pizza = i.Pizza == null ? null : new PizzaItemREST
                    {
                        PizzaId = i.Pizza.PizzaId,
                        Name = i.Pizza.Name,
                        Size = i.Pizza.Size,
                        Price = i.Pizza.Price,
                        IsVegetarian = i.Pizza.IsVegetarian
                    }

                }).ToList()
            });

            return Ok(ordersREST);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _pizzaOrderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order); // Ili mapiraj u REST model ako želiš
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] PizzaOrder order)
        {
            var orderId = await _pizzaOrderService.CreateOrderAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, null);
        }



        

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var success = await _pizzaOrderService.DeleteOrderAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }

    }
}
