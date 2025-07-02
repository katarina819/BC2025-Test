using BootcampApp.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BootcampApp.Repository
{
    /// <summary>
    /// Repository interface for managing pizza orders.
    /// </summary>
    public interface IPizzaOrderRepository
    {
        /// <summary>
        /// Retrieves a pizza order by its unique identifier.
        /// </summary>
        /// <param name="orderId">The ID of the pizza order.</param>
        /// <returns>
        /// A <see cref="PizzaOrder"/> if found; otherwise, <c>null</c>.
        /// </returns>
        Task<PizzaOrder?> GetByIdAsync(Guid orderId);

        /// <summary>
        /// Retrieves all pizza orders including their related details (e.g., order items, customer info).
        /// </summary>
        /// <returns>An enumerable collection of pizza orders with details.</returns>
        Task<IEnumerable<PizzaOrder>> GetPizzaOrdersWithDetailsAsync();

        /// <summary>
        /// Creates a new pizza order.
        /// </summary>
        /// <param name="order">The pizza order to create.</param>
        /// <returns>The unique identifier of the newly created pizza order.</returns>
        Task<Guid> CreateAsync(PizzaOrder order);

        /// <summary>
        /// Deletes an existing pizza order by its ID.
        /// </summary>
        /// <param name="orderId">The ID of the pizza order to delete.</param>
        /// <returns><c>true</c> if the order was deleted; otherwise, <c>false</c>.</returns>
        Task<bool> DeleteAsync(Guid orderId);
    }
}
