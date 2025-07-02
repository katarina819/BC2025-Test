using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;

namespace BootcampApp.Service.BootcampApp.Service.PizzaService
{
    /// <summary>
    /// Provides methods to manage pizza items, including retrieval of all pizzas and by ID.
    /// </summary>
    public interface IPizzaService
    {
        /// <summary>
        /// Retrieves all pizza items asynchronously.
        /// </summary>
        /// <returns>A list of all <see cref="PizzaItem"/> objects.</returns>
        Task<List<PizzaItem>> GetAllAsync();

        /// <summary>
        /// Retrieves a specific pizza item by its unique identifier asynchronously.
        /// </summary>
        /// <param name="pizzaId">The unique identifier of the pizza item.</param>
        /// <returns>The <see cref="PizzaItem"/> if found; otherwise, null.</returns>
        Task<PizzaItem?> GetByIdAsync(Guid pizzaId);
    }
}
