using System;
using System.Threading.Tasks;
using BootcampApp.Model;

namespace BootcampApp.Repository
{
    /// <summary>
    /// Defines data access operations for retrieving drink information
    /// from the underlying data store.
    /// </summary>
    public interface IDrinkRepository
    {
        /// <summary>
        /// Retrieves a drink by its unique identifier.
        /// </summary>
        /// <param name="drinkId">The unique identifier of the drink.</param>
        /// <returns>A <see cref="Drink"/> object if found; otherwise, <c>null</c>.</returns>
        Task<Drink?> GetByIdAsync(Guid drinkId);

        /// <summary>
        /// Retrieves all drinks available in the system.
        /// </summary>
        /// <returns>A list of all <see cref="Drink"/> entities.</returns>
        Task<List<Drink>> GetAllAsync();
    }
}
