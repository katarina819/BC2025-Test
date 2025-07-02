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
    /// <summary>
    /// Provides services for managing pizza orders, including retrieval, creation, and deletion.
    /// </summary>
    public class PizzaOrderService : IPizzaOrderService
    {
        private readonly IPizzaOrderRepository _pizzaOrderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPizzaRepository _pizzaRepository;
        private readonly ILogger<PizzaOrderService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PizzaOrderService"/> class.
        /// </summary>
        /// <param name="pizzaOrderRepository">Repository for pizza orders.</param>
        /// <param name="pizzaRepository">Repository for pizza items.</param>
        /// <param name="userRepository">Repository for users.</param>
        /// <param name="logger">Logger instance.</param>
        public PizzaOrderService(
            IPizzaOrderRepository pizzaOrderRepository,
            IPizzaRepository pizzaRepository,
            IUserRepository userRepository,
            ILogger<PizzaOrderService> logger)
        {
            _pizzaOrderRepository = pizzaOrderRepository;
            _pizzaRepository = pizzaRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all pizza orders including user and pizza details asynchronously.
        /// </summary>
        /// <returns>A collection of <see cref="PizzaOrder"/> with detailed information.</returns>
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

        /// <summary>
        /// Retrieves pizza orders with details from the repository asynchronously.
        /// </summary>
        /// <returns>A collection of <see cref="PizzaOrder"/>.</returns>
        public async Task<IEnumerable<PizzaOrder>> GetPizzaOrdersWithDetailsAsync()
        {
            return await _pizzaOrderRepository.GetPizzaOrdersWithDetailsAsync();
        }

        /// <summary>
        /// Retrieves a pizza order by its unique identifier, including user and pizza item details.
        /// </summary>
        /// <param name="orderId">The unique identifier of the pizza order.</param>
        /// <returns>The <see cref="PizzaOrder"/> if found; otherwise, null.</returns>
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

        /// <summary>
        /// Creates a new pizza order asynchronously based on the given request.
        /// </summary>
        /// <param name="request">The request containing order details.</param>
        /// <returns>The newly created <see cref="PizzaOrder"/>.</returns>
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

        /// <summary>
        /// Deletes a pizza order by its unique identifier asynchronously.
        /// </summary>
        /// <param name="orderId">The unique identifier of the pizza order to delete.</param>
        /// <returns><c>true</c> if the deletion was successful; otherwise, <c>false</c>.</returns>
        public async Task<bool> DeleteOrderAsync(Guid orderId)
        {
            return await _pizzaOrderRepository.DeleteAsync(orderId);
        }
    }
}
