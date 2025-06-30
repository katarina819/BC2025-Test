using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Common.BootcampApp.Common.DTOs;
using BootcampApp.Model;
using static BootcampApp.Model.PizzaOrder;

namespace BootcampApp.Service
{
    public interface IDrinksOrderService
    {
        Task<DrinksOrder?> GetOrderByIdAsync(Guid orderId);
        Task<List<DrinksOrder>> GetAllAsync();

        Task<DrinksOrder> CreateOrderAsync(CreateDrinksOrderRequest request);
        Task<bool> DeleteOrderAsync(Guid orderId);
        
    }
}

