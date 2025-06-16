using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;
using BootcampApp.Repository;
using Microsoft.Extensions.Logging;
using static BootcampApp.Model.PizzaOrder;

namespace BootcampApp.Service
{
    public class DrinksOrderService : IDrinksOrderService
    {
        private readonly IDrinksOrderRepository _repository;
        private readonly ILogger<DrinksOrderService> _logger;

        public DrinksOrderService(IDrinksOrderRepository repository, ILogger<DrinksOrderService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<DrinksOrder?> GetOrderByIdAsync(Guid orderId)
        {
            try
            {
                return await _repository.GetByIdAsync(orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to get drinks order with ID {orderId}");
                throw;
            }
        }

        public IEnumerable<DrinksOrder> GetDrinksOrdersWithDetails()
        {
            try
            {
                return _repository.GetDrinksOrdersWithDetails();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get drinks orders");
                throw;
            }
        }

        public async Task<Guid> CreateOrderAsync(DrinksOrder order)
        {
            try
            {
                order.OrderDate = DateTime.UtcNow;
                return await _repository.CreateAsync(order);
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
                return await _repository.DeleteAsync(orderId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete drinks order with ID {orderId}");
                throw;
            }
        }
    }
}

