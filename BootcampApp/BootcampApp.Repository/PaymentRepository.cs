using System;
using System.Threading.Tasks;
using BootcampApp.Model;
using Npgsql;

namespace BootcampApp.Repository
{
    public class PaymentRepository
    {
        private readonly NpgsqlConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentRepository"/> class with the specified database connection.
        /// </summary>
        /// <param name="connection">An open or closed NpgsqlConnection instance.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connection"/> is null.</exception>
        public PaymentRepository(NpgsqlConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Adds a new payment record to the database asynchronously.
        /// </summary>
        /// <param name="payment">The payment entity to add.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task AddPaymentAsync(Payment payment)
        {
            string sql = @"
                INSERT INTO payments (order_id, payment_method_id, amount, payment_date, order_type)
                VALUES (@OrderId, @PaymentMethodId, @Amount, @PaymentDate, @OrderType);
            ";

            if (_connection.State != System.Data.ConnectionState.Open)
                await _connection.OpenAsync();

            using var cmd = new NpgsqlCommand(sql, _connection);
            cmd.Parameters.AddWithValue("OrderId", payment.OrderId);
            cmd.Parameters.AddWithValue("PaymentMethodId", payment.PaymentMethodId);
            cmd.Parameters.AddWithValue("Amount", payment.Amount);
            cmd.Parameters.AddWithValue("PaymentDate", payment.PaymentDate);
            cmd.Parameters.AddWithValue("OrderType", payment.OrderType ?? (object)DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Checks asynchronously whether an order exists in the database based on order ID and type.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order.</param>
        /// <param name="orderType">The type of the order. Valid values are "pizza" or "drink".</param>
        /// <returns>
        /// <c>true</c> if the order exists; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="orderType"/> is invalid.</exception>
        public async Task<bool> OrderExistsAsync(Guid orderId, string orderType)
        {
            var tableName = orderType switch
            {
                "pizza" => "pizza_orders",
                "drink" => "drinks_orders",
                _ => throw new ArgumentException("Invalid order type", nameof(orderType))
            };

            var query = $"SELECT COUNT(*) FROM {tableName} WHERE \"OrderId\" = @OrderId";

            if (_connection.State != System.Data.ConnectionState.Open)
                await _connection.OpenAsync();

            using var command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("OrderId", orderId);

            var result = (long)await command.ExecuteScalarAsync();
            return result > 0;
        }
    }
}
