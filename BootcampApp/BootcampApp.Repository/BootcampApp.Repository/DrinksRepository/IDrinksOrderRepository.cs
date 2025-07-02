using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;
using static BootcampApp.Model.PizzaOrder;

namespace BootcampApp.Repository
{
    /// <summary>
    /// Defines operations for managing drink orders in the data store.
    /// </summary>
    public interface IDrinksOrderRepository
    {
        /// <summary>
        /// Retrieves a single drink order by its unique identifier.
        /// </summary>
        /// <param name="orderId">The unique identifier of the drink order.</param>
        /// <returns>
        /// A <see cref="DrinksOrder"/> object if found; otherwise, <c>null</c>.
        /// </returns>
        Task<DrinksOrder?> GetByIdAsync(Guid orderId);

        /// <summary>
        /// Retrieves all drink orders, including related user and item details.
        /// </summary>
        /// <returns>A list of all <see cref="DrinksOrder"/> records.</returns>
        Task<List<DrinksOrder>> GetAllAsync();

        /// <summary>
        /// Creates a new drink order along with its associated items.
        /// </summary>
        /// <param name="order">The <see cref="DrinksOrder"/> to be created.</param>
        /// <returns>The unique identifier (GUID) of the newly created order.</returns>
        Task<Guid> CreateAsync(DrinksOrder order);

        /// <summary>
        /// Deletes an existing drink order and all of its associated items.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to be deleted.</param>
        /// <returns><c>true</c> if the order was deleted successfully; otherwise, <c>false</c>.</returns>
        Task<bool> DeleteAsync(Guid orderId);
    }
}
