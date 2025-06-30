using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Common.BootcampApp.Common.DTOs;
using BootcampApp.Model;

namespace BootcampApp.Service
{
    public interface IPizzaOrderService
    {
        Task<PizzaOrder?> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<PizzaOrder>> GetAllOrdersWithDetailsAsync();
        Task<PizzaOrder> CreateOrderAsync(CreatePizzaOrderRequest request);
        Task<bool> DeleteOrderAsync(Guid orderId);

        
        Task<IEnumerable<PizzaOrder>> GetPizzaOrdersWithDetailsAsync();
        
    }
}


