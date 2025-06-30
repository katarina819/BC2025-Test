using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Common.BootcampApp.Common.DTOs;
using BootcampApp.Model;
using BootcampApp.Repository;
using Microsoft.Extensions.Logging;
using PizzaOrderItemNew = BootcampApp.Model.Entities.Pizza.PizzaOrderItem;



namespace BootcampApp.Service
{
    public class PizzaOrderService : IPizzaOrderService
    {
        private readonly IPizzaOrderRepository _pizzaOrderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPizzaRepository _pizzaRepository;
        private readonly ILogger<PizzaOrderService> _logger;

        public PizzaOrderService(
            IPizzaOrderRepository pizzaOrderRepository,
            IPizzaRepository pizzaRepository,
            IUserRepository userRepository, // dodaj ako koristiš
            ILogger<PizzaOrderService> logger)
        {
            _pizzaOrderRepository = pizzaOrderRepository;
            _pizzaRepository = pizzaRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<PizzaOrder>> GetAllOrdersWithDetailsAsync()
        {
            var orders = await _pizzaOrderRepository.GetPizzaOrdersWithDetailsAsync();

            foreach (var order in orders)
            {
                order.User = await _userRepository.GetByIdAsync(order.UserId);
                foreach (var item in order.Items)
                {
                    item.Pizza = await _pizzaRepository.GetByIdAsync(item.PizzaId); 
                }
            }

            return orders;
        }


        
        public async Task<IEnumerable<PizzaOrder>> GetPizzaOrdersWithDetailsAsync()
        {
            return await _pizzaOrderRepository.GetPizzaOrdersWithDetailsAsync();
        }

        public async Task<PizzaOrder?> GetOrderByIdAsync(Guid orderId)
        {
            var order = await _pizzaOrderRepository.GetByIdAsync(orderId);
            if (order == null)
                return null;

            order.User = await _userRepository.GetByIdAsync(order.UserId);
            foreach (var item in order.Items)
            {
                item.Pizza = await _pizzaRepository.GetByIdAsync(item.PizzaId);
            }
            return order;
        }

        public async Task<PizzaOrder> CreateOrderAsync(CreatePizzaOrderRequest request)
        {
            var newOrder = new PizzaOrder
            {
                OrderId = Guid.NewGuid(),
                UserId = request.UserId,
                OrderDate = DateTime.UtcNow,
                Items = new List<PizzaOrderItem>()
            };

            foreach (var item in request.Items)
            {
                var pizza = await _pizzaRepository.GetByIdAsync(item.PizzaId);
                var unitPrice = pizza.Price;

                var orderItem = new PizzaOrderItem

                {
                    OrderId = newOrder.OrderId,
                    PizzaId = item.PizzaId,
                    Quantity = item.Quantity,
                    UnitPrice = unitPrice,
                    TotalPrice = unitPrice * item.Quantity
                };

                newOrder.Items.Add(orderItem);
            }

            await _pizzaOrderRepository.CreateAsync(newOrder);
            return newOrder;
        }



        public async Task<bool> DeleteOrderAsync(Guid orderId)
        {
            return await _pizzaOrderRepository.DeleteAsync(orderId);
        }

       
    }
}
