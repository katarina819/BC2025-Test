using BootcampApp.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BootcampApp.Service
{
    public interface IPizzaOrderService
    {
        Task<PizzaOrder?> GetOrderByIdAsync(Guid orderId);
        Task<IEnumerable<PizzaOrder>> GetAllOrdersWithDetailsAsync();
        Task<Guid> CreateOrderAsync(PizzaOrder order);
        Task<bool> DeleteOrderAsync(Guid orderId);

        // Dodaj ako ti treba eksplicitna metoda za vraćanje svih narudžbi s detaljima
        Task<IEnumerable<PizzaOrder>> GetPizzaOrdersWithDetailsAsync();
    }
}


