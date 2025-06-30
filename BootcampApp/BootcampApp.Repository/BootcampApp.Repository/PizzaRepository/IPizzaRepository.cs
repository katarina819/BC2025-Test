using BootcampApp.Model;
using System;
using System.Threading.Tasks;

namespace BootcampApp.Repository
{
    public interface IPizzaRepository
    {
        Task<PizzaItem?> GetByIdAsync(Guid pizzaId);
        Task<List<PizzaItem>> GetAllAsync();
    }
}


