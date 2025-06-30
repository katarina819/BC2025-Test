using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BootcampApp.Model;
using Npgsql;

namespace BootcampApp.Repository
{
    public class PaymentRepository
    {
        private readonly NpgsqlConnection _connection;

        public PaymentRepository(NpgsqlConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            string sql = @"
        INSERT INTO payments (order_id, payment_method_id, amount, payment_date, order_type)
        VALUES (@OrderId, @PaymentMethodId, @Amount, @PaymentDate, @OrderType);
    ";

            if (_connection.State != System.Data.ConnectionState.Open)
                await _connection.OpenAsync(); // ← Ovo dodaj!

            using var cmd = new NpgsqlCommand(sql, _connection);
            cmd.Parameters.AddWithValue("OrderId", payment.OrderId);
            cmd.Parameters.AddWithValue("PaymentMethodId", payment.PaymentMethodId);
            cmd.Parameters.AddWithValue("Amount", payment.Amount);
            cmd.Parameters.AddWithValue("PaymentDate", payment.PaymentDate);
            cmd.Parameters.AddWithValue("OrderType", payment.OrderType ?? (object)DBNull.Value); // sigurna fallback

            await cmd.ExecuteNonQueryAsync();
        }



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
                await _connection.OpenAsync(); // Obavezno otvori vezu ako nije otvorena

            using var command = new NpgsqlCommand(query, _connection);
            command.Parameters.AddWithValue("OrderId", orderId);

            var result = (long)await command.ExecuteScalarAsync();
            return result > 0;
        }


    }

}
