using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Common.BootcampApp.Common.DTOs;
using BootcampApp.Model;
using BootcampApp.Repository;
using Microsoft.Extensions.Logging;

namespace BootcampApp.Service
{
    public class DrinksOrderService : IDrinksOrderService
    {
        private readonly IDrinksOrderRepository _orderRepository;
        private readonly IDrinkRepository _drinkRepository;
        private readonly ILogger<DrinksOrderService> _logger;
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinksOrderService"/> class.
        /// </summary>
        /// <param name="orderRepository">Repository for managing drink orders.</param>
        /// <param name="drinkRepository">Repository for managing drinks.</param>
        /// <param name="logger">Logger instance for logging errors and information.</param>
        /// <param name="notificationService">Service to handle notifications.</param>
        public DrinksOrderService(
            IDrinksOrderRepository orderRepository,
            IDrinkRepository drinkRepository,
            ILogger<DrinksOrderService> logger,
            INotificationService notificationService)
        {
            _orderRepository = orderRepository;
            _drinkRepository = drinkRepository;
            _logger = logger;
            _notificationService = notificationService;
        }

        /// <summary>
        /// Retrieves a drinks order by its unique identifier asynchronously.
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve.</param>
        /// <returns>The <see cref="DrinksOrder"/> if found; otherwise, <c>null</c>.</returns>
        /// <exception cref="Exception">Propagates exceptions that occur during retrieval.</exception>
        public async Task<DrinksOrder?> GetOrderByIdAsync(Guid orderId)
        {
            try
            {
                return await _orderRepository.GetByIdAsync(orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get drinks order with ID {orderId}");
                throw;
            }
        }

        /// <summary>
        /// Retrieves all drinks orders asynchronously.
        /// </summary>
        /// <returns>A list of all <see cref="DrinksOrder"/> instances.</returns>
        public async Task<List<DrinksOrder>> GetAllAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

        /// <summary>
        /// Creates a new drinks order asynchronously.
        /// </summary>
        /// <param name="request">The request DTO containing order details.</param>
        /// <returns>The created <see cref="DrinksOrder"/>.</returns>
        /// <exception cref="Exception">Thrown if any referenced drink is not found or if creation fails.</exception>
        public async Task<DrinksOrder> CreateOrderAsync(CreateDrinksOrderRequest request)
        {
            try
            {
                var newOrder = new DrinksOrder
                {
                    OrderId = Guid.NewGuid(),
                    UserId = request.UserId,
                    OrderDate = DateTime.UtcNow,
                    CardPaymentTransactionId = request.CardPaymentTransactionId,
                    Items = new List<DrinkOrderItem>()
                };

                foreach (var itemDto in request.Items)
                {
                    var drink = await _drinkRepository.GetByIdAsync(itemDto.DrinkId);
                    if (drink == null)
                    {
                        throw new Exception($"Drink with ID {itemDto.DrinkId} not found.");
                    }

                    var orderItem = new DrinkOrderItem
                    {
                        OrderItemId = Guid.NewGuid(),
                        OrderId = newOrder.OrderId,
                        DrinkId = drink.DrinkId,
                        Quantity = itemDto.Quantity,
                        UnitPrice = drink.Price,
                        Drink = drink
                    };

                    newOrder.Items.Add(orderItem);
                }

                await _orderRepository.CreateAsync(newOrder);

                // Add notification about the new order
                var message = $"Your order #{newOrder.OrderId} has been confirmed. Transaction ID: {request.CardPaymentTransactionId}";
                var link = $"/orders/drinks/{newOrder.OrderId}";

                await _notificationService.CreateNotificationAsync(newOrder.UserId, message, link);

                return newOrder;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create drinks order");
                throw;
            }
        }

        /// <summary>
        /// Deletes a drinks order by its ID asynchronously.
        /// </summary>
        /// <param name="orderId">The ID of the order to delete.</param>
        /// <returns><c>true</c> if the deletion was successful; otherwise, <c>false</c>.</returns>
        /// <exception cref="Exception">Propagates exceptions that occur during deletion.</exception>
        public async Task<bool> DeleteOrderAsync(Guid orderId)
        {
            try
            {
                return await _orderRepository.DeleteAsync(orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete drinks order with ID {orderId}");
                throw;
            }
        }
    }
}
