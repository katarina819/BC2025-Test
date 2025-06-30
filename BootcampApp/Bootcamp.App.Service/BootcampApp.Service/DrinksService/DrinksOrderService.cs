using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Common.BootcampApp.Common.DTOs;
using BootcampApp.Model;
using BootcampApp.Repository;
using Microsoft.Extensions.Logging;
using static BootcampApp.Model.PizzaOrder;

namespace BootcampApp.Service
{
    public class DrinksOrderService : IDrinksOrderService
    {
        private readonly IDrinksOrderRepository _orderRepository;
        private readonly IDrinkRepository _drinkRepository;
        private readonly ILogger<DrinksOrderService> _logger;
        private readonly INotificationService _notificationService;


        public DrinksOrderService(IDrinksOrderRepository orderRepository,
    IDrinkRepository drinkRepository,
    ILogger<DrinksOrderService> logger, INotificationService notificationService)
        {
            _orderRepository = orderRepository;
            _drinkRepository = drinkRepository;
            _logger = logger;
            _notificationService = notificationService;
        }

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

        public async Task<List<DrinksOrder>> GetAllAsync()
        {
            return await _orderRepository.GetAllAsync();
        }

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

                // Dodaj notifikaciju
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

