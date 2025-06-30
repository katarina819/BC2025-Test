using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Common.BootcampApp.Common.DTOs;
using BootcampApp.Model;
using BootcampApp.Repository;
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
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var orders = await _drinksOrderService.GetAllAsync();

        //    var result = orders.Select(o => new
        //    {
        //        o.OrderId,
        //        o.OrderDate,
        //        user = new
        //        {
        //            o.User.Id,
        //            o.User.Name,
        //            o.User.Email,
        //            profile = new
        //            {
        //                o.User.Profile.PhoneNumber,
        //                o.User.Profile.Address
        //            }
        //        },
        //        items = o.Items.Select(i => new
        //        {
        //            i.Quantity,
        //            i.UnitPrice,
        //            i.TotalCost,
        //            drink = new
        //            {
        //                i.Drink.DrinkId,
        //                i.Drink.Name,
        //                i.Drink.Size,
        //                i.Drink.Price
        //            }
        //        })
        //    });

        //    return Ok(result);
        //}


        // GET: api/DrinksOrder/{id}
        //[HttpGet("{id}")]
        //public async Task<ActionResult<DrinksOrder>> GetOrderById(Guid id)
        //{
        //    var order = await _drinksOrderService.GetOrderByIdAsync(id);
        //    if (order == null)
        //        return NotFound();
        //    return Ok(order);
        //}

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drink>>> GetAll()
        {
            var drinks = await _drinksOrderService.GetAllAsync();
            return Ok(drinks);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDrinksOrderRequest request)
        {
            if (request == null || request.Items == null || !request.Items.Any())
                return BadRequest("Order must contain at least one item.");

            if (request.Items.Any(i => i.Quantity <= 0))
                return BadRequest("Each drink must have a quantity greater than zero.");

            var createdOrder = await _drinksOrderService.CreateOrderAsync(request);
            return Ok(createdOrder);
        }





        // DELETE: api/DrinksOrder/{id}
        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteOrder(Guid id)
        //{
        //    var deleted = await _drinksOrderService.DeleteOrderAsync(id);
        //    if (!deleted)
        //        return NotFound();

        //    return NoContent();
        //}
    }
}

