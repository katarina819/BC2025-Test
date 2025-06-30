using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BootcampApp.Model;
using BootcampApp.Repository;
using Microsoft.Extensions.Logging;

namespace BootcampApp.Service.BootcampApp.Service.PizzaService
{
    public class PizzaService : IPizzaService
    {
        private readonly IPizzaRepository _pizzaRepository;
        private readonly ILogger<PizzaService> _logger;

        public PizzaService(IPizzaRepository pizzaRepository, ILogger<PizzaService> logger)
        {
            _pizzaRepository = pizzaRepository;
            _logger = logger;
        }

        public async Task<List<PizzaItem>> GetAllAsync()
        {
            try
            {
                return await _pizzaRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all pizzas");
                throw;
            }
        }

        public async Task<PizzaItem?> GetByIdAsync(Guid pizzaId)
        {
            try
            {
                return await _pizzaRepository.GetByIdAsync(pizzaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting pizza by id: {pizzaId}");
                throw;
            }
        }
    }
}

