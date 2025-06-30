using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BootcampApp.Model;

namespace BootcampApp.Service.BootcampApp.Service.PizzaService
{
    public interface IPizzaService
    {
        Task<List<PizzaItem>> GetAllAsync();
        Task<PizzaItem?> GetByIdAsync(Guid pizzaId);
    }
}
