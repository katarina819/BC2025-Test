using BootcampApp.Model;
using BootcampApp.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Service.BootcampApp.Service.PizzaService;

namespace BootcampApp.Controllers
{
    /// <summary>
    /// API controller for managing pizzas.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PizzaController : ControllerBase
    {
        private readonly IPizzaService _pizzaService;

        /// <summary>
        /// Initializes a new instance of the <see cref="PizzaController"/> class.
        /// </summary>
        /// <param name="pizzaService">Service to handle pizza-related operations.</param>
        public PizzaController(IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        /// <summary>
        /// Retrieves all available pizzas.
        /// </summary>
        /// <returns>A list of all pizzas.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PizzaItem>>> GetAll()
        {
            var pizzas = await _pizzaService.GetAllAsync();
            return Ok(pizzas);
        }

        /// <summary>
        /// Retrieves a pizza by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the pizza.</param>
        /// <returns>The pizza if found; otherwise, NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PizzaItem>> GetById(Guid id)
        {
            var pizza = await _pizzaService.GetByIdAsync(id);
            if (pizza == null)
                return NotFound();
            return Ok(pizza);
        }
    }
}
