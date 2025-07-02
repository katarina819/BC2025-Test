using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;
using BootcampApp.Repository;
using Microsoft.Extensions.Logging;

namespace BootcampApp.Service.BootcampApp.Service.PizzaService
{
    /// <summary>
    /// Provides services for retrieving pizza items.
    /// </summary>
    public class PizzaService : IPizzaService
    {
        private readonly IPizzaRepository _pizzaRepository;
        private readonly ILogger<PizzaService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PizzaService"/> class.
        /// </summary>
        /// <param name="pizzaRepository">The pizza repository.</param>
        /// <param name="logger">The logger instance.</param>
        public PizzaService(IPizzaRepository pizzaRepository, ILogger<PizzaService> logger)
        {
            _pizzaRepository = pizzaRepository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all pizza items asynchronously.
        /// </summary>
        /// <returns>A list of all <see cref="PizzaItem"/> objects.</returns>
        /// <exception cref="Exception">Throws exception if the retrieval fails.</exception>
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

        /// <summary>
        /// Retrieves a pizza item by its unique identifier asynchronously.
        /// </summary>
        /// <param name="pizzaId">The unique identifier of the pizza item.</param>
        /// <returns>The <see cref="PizzaItem"/> if found; otherwise, null.</returns>
        /// <exception cref="Exception">Throws exception if the retrieval fails.</exception>
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
