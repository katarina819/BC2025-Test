using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BootcampApp.Model;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace BootcampApp.Repository
{
    /// <summary>
    /// Repository for managing drinks orders in the database.
    /// Provides methods to retrieve, create, and delete orders and their items.
    /// </summary>
    public class DrinksOrderRepository : IDrinksOrderRepository
    {
        private readonly ILogger<DrinksOrderRepository> _logger;
        private readonly string _connectionString;

        public DrinksOrderRepository(string connectionString, ILogger<DrinksOrderRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a single drinks order by ID, including its associated items.
        /// </summary>
        /// <param name="orderId">Unique identifier of the drinks order.</param>
        /// <returns>The drinks order with its items, or null if not found.</returns>
        public async Task<DrinksOrder?> GetByIdAsync(Guid orderId)
        {
            DrinksOrder? order = null;

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            // First, retrieve the basic order information
            const string orderQuery = @"
                SELECT ""OrderId"", ""UserId"", ""OrderDate"", ""CardPaymentTransactionId""
                FROM drinks_orders
                WHERE ""OrderId"" = @OrderId";

            await using (var cmd = new NpgsqlCommand(orderQuery, connection))
            {
                cmd.Parameters.AddWithValue("OrderId", orderId);

                await using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    order = new DrinksOrder
                    {
                        OrderId = reader.GetGuid(0),
                        UserId = reader.GetGuid(1),
                        OrderDate = reader.GetDateTime(2),
                        CardPaymentTransactionId = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Items = new List<DrinkOrderItem>()
                    };
                }
                else
                {
                    return null; // Order not found
                }
            }

            // Then, retrieve the associated order items
            const string itemsQuery = @"
                SELECT ""OrderItemId"", ""OrderId"", ""DrinkId"", ""Quantity"", ""UnitPrice""
                FROM ""DrinkOrderItems""
                WHERE ""OrderId"" = @OrderId";

            await using (var cmdItems = new NpgsqlCommand(itemsQuery, connection))
            {
                cmdItems.Parameters.AddWithValue("OrderId", orderId);

                await using var reader = await cmdItems.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var item = new DrinkOrderItem
                    {
                        OrderItemId = reader.GetGuid(0),
                        OrderId = reader.GetGuid(1),
                        DrinkId = reader.GetGuid(2),
                        Quantity = reader.GetInt32(3),
                        UnitPrice = reader.GetDecimal(4),
                        Drink = null! // Will be loaded later if needed
                    };

                    order.Items.Add(item);
                }
            }

            return order;
        }

        /// <summary>
        /// Retrieves all drinks orders with their users and drink item details.
        /// </summary>
        /// <returns>A list of drinks orders with related data.</returns>
        public async Task<List<DrinksOrder>> GetAllAsync()
        {
            var orders = new List<DrinksOrder>();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            // Complex join to gather orders, user info, order items and drink details
            string sql = @"
                SELECT
                    o.""OrderId"",
                    o.""OrderDate"",
                    u.""Id"" AS ""UserId"",
                    u.""Name"",
                    u.""Email"",
                    up.""PhoneNumber"",
                    up.""Address"",
                    i.""DrinkId"",
                    i.""Quantity"",
                    i.""UnitPrice"",
                    d.""Name"" AS ""DrinkName"",
                    d.""Size"",
                    d.""Price""
                FROM drinks_orders o
                JOIN ""Users"" u ON o.""UserId"" = u.""Id""
                LEFT JOIN ""UserProfiles"" up ON u.""Id"" = up.""UserId""
                LEFT JOIN ""DrinkOrderItems"" i ON o.""OrderId"" = i.""OrderId""
                LEFT JOIN ""Drinks"" d ON i.""DrinkId"" = d.""DrinkId""
                ORDER BY o.""OrderDate"", o.""OrderId"";";

            await using var command = new NpgsqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var orderId = reader.GetGuid(reader.GetOrdinal("OrderId"));

                // Check if order already exists in the result set
                var existingOrder = orders.FirstOrDefault(o => o.OrderId == orderId);
                if (existingOrder == null)
                {
                    // Build user and order only once
                    var user = new User
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("UserId")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Profile = new UserProfile
                        {
                            PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? "N/A" : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                            Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? "N/A" : reader.GetString(reader.GetOrdinal("Address"))
                        }
                    };

                    existingOrder = new DrinksOrder
                    {
                        OrderId = orderId,
                        UserId = user.Id,
                        OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                        User = user,
                        Items = new List<DrinkOrderItem>()
                    };

                    orders.Add(existingOrder);
                }

                // Add item if it exists
                if (!reader.IsDBNull(reader.GetOrdinal("DrinkId")))
                {
                    var drink = new Drink
                    {
                        DrinkId = reader.GetGuid(reader.GetOrdinal("DrinkId")),
                        Name = reader.GetString(reader.GetOrdinal("DrinkName")),
                        Size = reader.IsDBNull(reader.GetOrdinal("Size")) ? "Unknown" : reader.GetString(reader.GetOrdinal("Size")),
                        Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? 0 : reader.GetDecimal(reader.GetOrdinal("Price"))
                    };

                    var item = new DrinkOrderItem
                    {
                        DrinkId = drink.DrinkId,
                        Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                        UnitPrice = reader.GetDecimal(reader.GetOrdinal("UnitPrice")),
                        Drink = drink
                    };

                    existingOrder.Items.Add(item);
                }
            }

            return orders;
        }

        /// <summary>
        /// Creates a new drinks order with its associated items.
        /// </summary>
        /// <param name="order">Order object to insert into the database.</param>
        /// <returns>Generated order ID.</returns>
        public async Task<Guid> CreateAsync(DrinksOrder order)
        {
            order.OrderId = Guid.NewGuid();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // Insert the drinks order
                var insertOrderCmd = new NpgsqlCommand(@"
                    INSERT INTO drinks_orders (""OrderId"", ""UserId"", ""OrderDate"", ""CardPaymentTransactionId"")
                    VALUES (@OrderId, @UserId, @OrderDate, @CardPaymentTransactionId)", connection, transaction);

                insertOrderCmd.Parameters.AddWithValue("OrderId", order.OrderId);
                insertOrderCmd.Parameters.AddWithValue("UserId", order.UserId);
                insertOrderCmd.Parameters.AddWithValue("OrderDate", order.OrderDate);
                insertOrderCmd.Parameters.AddWithValue("CardPaymentTransactionId",
                    (object?)order.CardPaymentTransactionId ?? DBNull.Value);

                await insertOrderCmd.ExecuteNonQueryAsync();

                // Insert each order item
                foreach (var item in order.Items)
                {
                    item.OrderItemId = Guid.NewGuid();

                    var insertItemCmd = new NpgsqlCommand(@"
                        INSERT INTO ""DrinkOrderItems"" (""OrderItemId"", ""OrderId"", ""DrinkId"", ""Quantity"", ""UnitPrice"")
                        VALUES (@OrderItemId, @OrderId, @DrinkId, @Quantity, @UnitPrice)", connection, transaction);

                    insertItemCmd.Parameters.AddWithValue("OrderItemId", item.OrderItemId);
                    insertItemCmd.Parameters.AddWithValue("OrderId", order.OrderId);
                    insertItemCmd.Parameters.AddWithValue("DrinkId", item.DrinkId);
                    insertItemCmd.Parameters.AddWithValue("Quantity", item.Quantity);
                    insertItemCmd.Parameters.AddWithValue("UnitPrice", item.UnitPrice);

                    await insertItemCmd.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
                return order.OrderId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating drinks order");
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Deletes a drinks order and all its associated items.
        /// </summary>
        /// <param name="orderId">ID of the order to delete.</param>
        /// <returns>True if the order was deleted; false otherwise.</returns>
        public async Task<bool> DeleteAsync(Guid orderId)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // Delete associated order items
                var deleteItemsCmd = new NpgsqlCommand(
                    @"DELETE FROM ""DrinkOrderItems"" WHERE ""OrderId"" = @OrderId", connection, transaction);
                deleteItemsCmd.Parameters.AddWithValue("OrderId", orderId);
                await deleteItemsCmd.ExecuteNonQueryAsync();

                // Delete the main order
                var deleteOrderCmd = new NpgsqlCommand(
                    @"DELETE FROM drinks_orders WHERE ""OrderId"" = @OrderId", connection, transaction);
                deleteOrderCmd.Parameters.AddWithValue("OrderId", orderId);
                int affected = await deleteOrderCmd.ExecuteNonQueryAsync();

                await transaction.CommitAsync();
                return affected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting drinks order");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
