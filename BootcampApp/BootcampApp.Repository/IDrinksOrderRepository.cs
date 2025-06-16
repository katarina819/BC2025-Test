using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;
using static BootcampApp.Model.PizzaOrder;

namespace BootcampApp.Repository
{
    public interface IDrinksOrderRepository
    {
        Task<DrinksOrder?> GetByIdAsync(Guid orderId);
        IEnumerable<DrinksOrder> GetDrinksOrdersWithDetails();
        Task<Guid> CreateAsync(DrinksOrder order);
        Task<bool> DeleteAsync(Guid orderId);
    }
}
