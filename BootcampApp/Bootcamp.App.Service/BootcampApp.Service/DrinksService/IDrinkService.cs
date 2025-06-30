using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BootcampApp.Model;
using BootcampApp.Repository;
using Microsoft.Extensions.Logging;

namespace BootcampApp.Service.BootcampApp.Service.DrinksService
{
    public interface IDrinkService
    {
        Task<List<Drink>> GetAllAsync();
        Task<Drink?> GetByIdAsync(Guid drinkId);
    }

    public class DrinkService : IDrinkService
    {
        private readonly IDrinkRepository _drinkRepository;
        private readonly ILogger<DrinkService> _logger;

        public DrinkService(IDrinkRepository drinkRepository, ILogger<DrinkService> logger)
        {
            _drinkRepository = drinkRepository;
            _logger = logger;
        }

        public async Task<List<Drink>> GetAllAsync() => await _drinkRepository.GetAllAsync();
        public async Task<Drink?> GetByIdAsync(Guid drinkId) => await _drinkRepository.GetByIdAsync(drinkId);
    }

}
