using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Common.BootcampApp.Common.DTOs;
using BootcampApp.Model;
using static BootcampApp.Model.PizzaOrder;

namespace BootcampApp.Service
{
    /// <summary>
    /// Defines the contract for managing drinks orders.
    /// </summary>
    public interface IDrinksOrderService
    {
        /// <summary>
        /// Retrieves a drinks order by its unique identifier asynchronously.
        /// </summary>
        /// <param name="orderId">The unique identifier of the drinks order.</param>
        /// <returns>The <see cref="DrinksOrder"/> if found; otherwise, <c>null</c>.</returns>
        Task<DrinksOrder?> GetOrderByIdAsync(Guid orderId);

        /// <summary>
        /// Retrieves all drinks orders asynchronously.
        /// </summary>
        /// <returns>A list of all <see cref="DrinksOrder"/> objects.</returns>
        Task<List<DrinksOrder>> GetAllAsync();

        /// <summary>
        /// Creates a new drinks order asynchronously.
        /// </summary>
        /// <param name="request">The request DTO containing details for creating the order.</param>
        /// <returns>The created <see cref="DrinksOrder"/>.</returns>
        Task<DrinksOrder> CreateOrderAsync(CreateDrinksOrderRequest request);

        /// <summary>
        /// Deletes a drinks order by its unique identifier asynchronously.
        /// </summary>
        /// <param name="orderId">The unique identifier of the drinks order to delete.</param>
        /// <returns><c>true</c> if the order was successfully deleted; otherwise, <c>false</c>.</returns>
        Task<bool> DeleteOrderAsync(Guid orderId);
    }
}
