using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;
using BootcampApp.Service;
using BootcampApp.Service.BootcampApp.Service.DrinksService;
using Microsoft.AspNetCore.Mvc;

namespace BootcampApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DrinkController : ControllerBase
    {
        private readonly IDrinkService _drinkService;

        public DrinkController(IDrinkService drinkService)
        {
            _drinkService = drinkService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Drink>>> GetAll()
        {
            var drinks = await _drinkService.GetAllAsync();
            return Ok(drinks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Drink>> GetById(Guid id)
        {
            var drink = await _drinkService.GetByIdAsync(id);
            if (drink == null)
                return NotFound();
            return Ok(drink);
        }

        // Po potrebi možeš dodati Create, Update, Delete metode
    }
}
