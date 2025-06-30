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
    [ApiController]
    [Route("api/[controller]")]
    public class PizzaOrdersController : ControllerBase
    {
        private readonly IPizzaService _pizzaService;
        private readonly IPizzaOrderService _pizzaOrderService;


        public PizzaOrdersController(IPizzaService pizzaService, IPizzaOrderService pizzaOrderService)
        {
            _pizzaService = pizzaService;
            _pizzaOrderService = pizzaOrderService;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetOrders()
        //{
        //    var orders = await _pizzaOrderService.GetAllOrdersWithDetailsAsync();


        //    var ordersREST = orders.Select(o => new PizzaOrderREST
        //    {
        //        OrderId = o.OrderId,
        //        OrderDate = o.OrderDate,
        //        User = new UserREST
        //        {
        //            Id = o.User.Id,
        //            Name = o.User.Name,
        //            Email = o.User.Email,
        //            Age = o.User.Age,
        //            Profile = o.User.Profile == null ? null : new UserProfileREST
        //            {
        //                PhoneNumber = o.User.Profile.PhoneNumber,
        //                Address = o.User.Profile.Address
        //            }
        //        },
        //        Items = o.Items.Select(i => new PizzaOrderItemREST
        //        {
        //            OrderItemId = i.OrderItemId,
        //            Quantity = i.Quantity,
        //            UnitPrice = i.UnitPrice,
        //            Pizza = i.Pizza == null ? null : new PizzaItemREST
        //            {
        //                PizzaId = i.Pizza.PizzaId,
        //                Name = i.Pizza.Name,
        //                Size = i.Pizza.Size,
        //                Price = i.Pizza.Price,
        //                IsVegetarian = i.Pizza.IsVegetarian
        //            }

        //        }).ToList()
        //    });

        //    return Ok(ordersREST);
        //}

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetOrderById(Guid id)
        //{
        //    var order = await _pizzaOrderService.GetOrderByIdAsync(id);
        //    if (order == null) return NotFound();
        //    return Ok(order); 
        //}


        [HttpGet]
        public async Task<ActionResult<IEnumerable<PizzaItem>>> GetAll()
        {
            var pizzas = await _pizzaService.GetAllAsync();
            return Ok(pizzas);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePizzaOrderRequest request)
        {
            if (request == null || request.Items == null || !request.Items.Any())
                return BadRequest("Order must contain at least one item.");

            if (request.Items.Any(i => i.Quantity <= 0))
                return BadRequest("Each item must have a quantity greater than zero.");

            var createdOrder = await _pizzaOrderService.CreateOrderAsync(request);

            // Vrati orderId u odgovoru:
            return Ok(new { orderId = createdOrder.OrderId });
        }








        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteOrder(Guid id)
        //{
        //    var success = await _pizzaOrderService.DeleteOrderAsync(id);
        //    if (!success) return NotFound();
        //    return NoContent();
        //}

    }
}
