using BootcampApp.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BootcampApp.Repository
{
    /// <summary>
    /// Repository interface for managing pizza items.
    /// </summary>
    public interface IPizzaRepository
    {
        /// <summary>
        /// Retrieves a pizza item by its unique identifier.
        /// </summary>
        /// <param name="pizzaId">The ID of the pizza item.</param>
        /// <returns>
        /// A <see cref="PizzaItem"/> if found; otherwise, <c>null</c>.
        /// </returns>
        Task<PizzaItem?> GetByIdAsync(Guid pizzaId);

        /// <summary>
        /// Retrieves all pizza items.
        /// </summary>
        /// <returns>A list of all pizza items.</returns>
        Task<List<PizzaItem>> GetAllAsync();
    }
}
