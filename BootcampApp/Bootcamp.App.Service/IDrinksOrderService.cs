using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;
using static BootcampApp.Model.PizzaOrder;

namespace BootcampApp.Service
{
    public interface IDrinksOrderService
    {
        Task<DrinksOrder?> GetOrderByIdAsync(Guid orderId);
        IEnumerable<DrinksOrder> GetDrinksOrdersWithDetails();

        Task<Guid> CreateOrderAsync(DrinksOrder order);
        Task<bool> DeleteOrderAsync(Guid orderId);
    }
}

