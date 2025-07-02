using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;
using BootcampApp.Repository;
using Microsoft.Extensions.Logging;

namespace BootcampApp.Service.BootcampApp.Service.DrinksService
{
    /// <summary>
    /// Defines the contract for managing drinks data.
    /// </summary>
    public interface IDrinkService
    {
        /// <summary>
        /// Retrieves all drinks asynchronously.
        /// </summary>
        /// <returns>A list of all available <see cref="Drink"/> objects.</returns>
        Task<List<Drink>> GetAllAsync();

        /// <summary>
        /// Retrieves a drink by its unique identifier asynchronously.
        /// </summary>
        /// <param name="drinkId">The unique ID of the drink.</param>
        /// <returns>The <see cref="Drink"/> if found; otherwise, <c>null</c>.</returns>
        Task<Drink?> GetByIdAsync(Guid drinkId);
    }

    /// <summary>
    /// Provides implementation of <see cref="IDrinkService"/> for managing drinks.
    /// </summary>
    public class DrinkService : IDrinkService
    {
        private readonly IDrinkRepository _drinkRepository;
        private readonly ILogger<DrinkService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkService"/> class.
        /// </summary>
        /// <param name="drinkRepository">Repository for drink data access.</param>
        /// <param name="logger">Logger instance for logging information and errors.</param>
        public DrinkService(IDrinkRepository drinkRepository, ILogger<DrinkService> logger)
        {
            _drinkRepository = drinkRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<List<Drink>> GetAllAsync() => await _drinkRepository.GetAllAsync();

        /// <inheritdoc />
        public async Task<Drink?> GetByIdAsync(Guid drinkId) => await _drinkRepository.GetByIdAsync(drinkId);
    }
}
