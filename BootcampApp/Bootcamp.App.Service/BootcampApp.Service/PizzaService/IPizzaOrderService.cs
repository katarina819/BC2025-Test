using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Common.BootcampApp.Common.DTOs;
using BootcampApp.Model;

namespace BootcampApp.Service
{
    /// <summary>
    /// Provides methods for managing pizza orders including retrieval, creation, and deletion.
    /// </summary>
    public interface IPizzaOrderService
    {
        /// <summary>
        /// Retrieves a pizza order by its unique identifier asynchronously.
        /// </summary>
        /// <param name="orderId">The unique identifier of the pizza order.</param>
        /// <returns>The <see cref="PizzaOrder"/> if found; otherwise, null.</returns>
        Task<PizzaOrder?> GetOrderByIdAsync(Guid orderId);

        /// <summary>
        /// Retrieves all pizza orders with their details asynchronously.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="PizzaOrder"/> objects with details.</returns>
        Task<IEnumerable<PizzaOrder>> GetAllOrdersWithDetailsAsync();

        /// <summary>
        /// Creates a new pizza order asynchronously based on the provided request data.
        /// </summary>
        /// <param name="request">The data transfer object containing the order details.</param>
        /// <returns>The created <see cref="PizzaOrder"/>.</returns>
        Task<PizzaOrder> CreateOrderAsync(CreatePizzaOrderRequest request);

        /// <summary>
        /// Deletes a pizza order by its unique identifier asynchronously.
        /// </summary>
        /// <param name="orderId">The unique identifier of the pizza order to delete.</param>
        /// <returns>True if the order was successfully deleted; otherwise, false.</returns>
        Task<bool> DeleteOrderAsync(Guid orderId);

        /// <summary>
        /// Retrieves pizza orders along with their details asynchronously.
        /// This may be similar or identical to <see cref="GetAllOrdersWithDetailsAsync"/>.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="PizzaOrder"/> objects with details.</returns>
        Task<IEnumerable<PizzaOrder>> GetPizzaOrdersWithDetailsAsync();
    }
}
