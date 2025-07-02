using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;
using BootcampApp.Service;
using BootcampApp.Service.BootcampApp.Service.DrinksService;
using Microsoft.AspNetCore.Mvc;

namespace BootcampApp.Controllers
{
    /// <summary>
    /// API controller for managing drink entities.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class DrinkController : ControllerBase
    {
        private readonly IDrinkService _drinkService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkController"/> class.
        /// </summary>
        /// <param name="drinkService">Service to manage drinks.</param>
        public DrinkController(IDrinkService drinkService)
        {
            _drinkService = drinkService;
        }

        /// <summary>
        /// Retrieves all drinks.
        /// </summary>
        /// <returns>A list of all drinks.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drink>>> GetAll()
        {
            var drinks = await _drinkService.GetAllAsync();
            return Ok(drinks);
        }

        /// <summary>
        /// Retrieves a drink by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the drink.</param>
        /// <returns>The drink with the specified ID if found; otherwise, NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Drink>> GetById(Guid id)
        {
            var drink = await _drinkService.GetByIdAsync(id);
            if (drink == null)
                return NotFound();
            return Ok(drink);
        }

        // Additional Create, Update, Delete methods can be added as needed.
    }
}
