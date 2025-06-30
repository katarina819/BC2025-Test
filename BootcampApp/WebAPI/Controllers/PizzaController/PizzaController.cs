using BootcampApp.Model;
using BootcampApp.Service;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Service.BootcampApp.Service.PizzaService;

namespace BootcampApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PizzaController : ControllerBase
    {
        private readonly IPizzaService _pizzaService;

        public PizzaController(IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        // GET: api/pizza
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PizzaItem>>> GetAll()
        {
            var pizzas = await _pizzaService.GetAllAsync();
            return Ok(pizzas);
        }

        // GET: api/pizza/{id}
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
