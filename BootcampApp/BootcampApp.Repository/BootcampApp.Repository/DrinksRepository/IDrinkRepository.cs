using System;
using System.Threading.Tasks;
using BootcampApp.Model;

namespace BootcampApp.Repository
{
    public interface IDrinkRepository
    {
        Task<Drink?> GetByIdAsync(Guid drinkId);
        Task<List<Drink>> GetAllAsync();

    }
}
