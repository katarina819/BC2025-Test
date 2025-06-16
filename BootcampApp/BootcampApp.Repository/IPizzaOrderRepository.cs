using BootcampApp.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BootcampApp.Repository
{
    public interface IPizzaOrderRepository
    {
        Task<PizzaOrder?> GetByIdAsync(Guid orderId);
        Task<IEnumerable<PizzaOrder>> GetPizzaOrdersWithDetailsAsync();

        Task<Guid> CreateAsync(PizzaOrder order);
        Task<bool> DeleteAsync(Guid orderId);
    }
}



