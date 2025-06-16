using BootcampApp.Model;
using BootcampApp.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BootcampApp.Service
{
    public class PizzaOrderService : IPizzaOrderService
    {
        private readonly IPizzaOrderRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IPizzaRepository _pizzaRepository;

        public PizzaOrderService(
            IPizzaOrderRepository repository,
            IUserRepository userRepository,
            IPizzaRepository pizzaRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
            _pizzaRepository = pizzaRepository;
        }

        // Ispravljena async metoda
        public async Task<IEnumerable<PizzaOrder>> GetAllOrdersWithDetailsAsync()
        {
            var orders = await _repository.GetPizzaOrdersWithDetailsAsync();

            foreach (var order in orders)
            {
                order.User = await _userRepository.GetByIdAsync(order.UserId);
                foreach (var item in order.Items)
                {
                    item.Pizza = await _pizzaRepository.GetByIdAsync(item.PizzaId); // <-- moguće da ovdje vraća null
                }
            }

            return orders;
        }


        // Ovu metodu ili makni ili je također napravi async verzijom
        // Ako ti ne treba, najbolje ju ukloni.
        public async Task<IEnumerable<PizzaOrder>> GetPizzaOrdersWithDetailsAsync()
        {
            return await _repository.GetPizzaOrdersWithDetailsAsync();
        }

        public async Task<PizzaOrder?> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _repository.GetByIdAsync(orderId);
            if (order == null)
                return null;

            order.User = await _userRepository.GetByIdAsync(order.UserId);
            foreach (var item in order.Items)
            {
                item.Pizza = await _pizzaRepository.GetByIdAsync(item.PizzaId);
            }
            return order;
        }

        public async Task<Guid> CreateOrderAsync(PizzaOrder order)
        {
            order.OrderDate = DateTime.UtcNow;
            return await _repository.CreateAsync(order);
        }

        public async Task<bool> DeleteOrderAsync(Guid orderId)
        {
            return await _repository.DeleteAsync(orderId);
        }
    }
}
