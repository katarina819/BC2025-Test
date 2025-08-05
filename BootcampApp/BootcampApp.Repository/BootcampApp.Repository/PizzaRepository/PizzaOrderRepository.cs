using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace BootcampApp.Repository
{
    /// <summary>
    /// Repository implementation for managing pizza orders using PostgreSQL database.
    /// </summary>
    public class PizzaOrderRepository : IPizzaOrderRepository
    {
        private readonly ILogger<PizzaOrderRepository> _logger;
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="PizzaOrderRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The database connection string.</param>
        /// <param name="logger">The logger instance for logging errors and information.</param>
        public PizzaOrderRepository(string connectionString, ILogger<PizzaOrderRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a pizza order by its unique identifier, including its order items.
        /// </summary>
        /// <param name="orderId">The unique identifier of the pizza order.</param>
        /// <returns>The <see cref="PizzaOrder"/> if found; otherwise, <c>null</c>.</returns>
        public async Task<PizzaOrder?> GetByIdAsync(Guid orderId)
        {
            PizzaOrder? order = null;

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            // Query to fetch the pizza order main data
            const string orderQuery = @"
                SELECT ""OrderId"", ""UserId"", ""OrderDate""
                FROM ""PizzaOrders"" 
                WHERE ""OrderId"" = @OrderId";

            await using (var cmd = new NpgsqlCommand(orderQuery, connection))
            {
                cmd.Parameters.AddWithValue("OrderId", orderId);

                await using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    order = new PizzaOrder
                    {
                        OrderId = reader.GetGuid(0),
                        UserId = reader.GetGuid(1),
                        OrderDate = reader.GetDateTime(2),
                        Items = new List<PizzaOrderItem>()
                    };
                }
                else
                {
                    // Order not found
                    return null;
                }
            }

            // Query to fetch the related order items for the found order
            const string itemsQuery = @"
                SELECT ""OrderItemId"", ""OrderId"", ""PizzaId"", ""Quantity"", ""UnitPrice""
                FROM ""PizzaOrderItems""
                WHERE ""OrderId"" = @OrderId";

            await using (var cmdItems = new NpgsqlCommand(itemsQuery, connection))
            {
                cmdItems.Parameters.AddWithValue("OrderId", orderId);

                await using var reader = await cmdItems.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var item = new PizzaOrderItem
                    {
                        OrderItemId = reader.GetGuid(0),
                        OrderId = reader.GetGuid(1),
                        PizzaId = reader.GetGuid(2),
                        Quantity = reader.GetInt32(3),
                        UnitPrice = reader.GetDecimal(4),
                        Pizza = null! // Pizza details not loaded here
                    };

                    order.Items.Add(item);
                }
            }

            return order;
        }

        /// <summary>
        /// Retrieves all pizza orders with their details, including user and order items with pizza info.
        /// </summary>
        /// <returns>A collection of pizza orders with details.</returns>
        public async Task<IEnumerable<PizzaOrder>> GetPizzaOrdersWithDetailsAsync()
        {
            var orders = new List<PizzaOrder>();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
        SELECT
            po.""OrderId"", po.""OrderDate"",
            u.""Id"", u.""Name"", u.""Email"", u.""Age"",
            up.""PhoneNumber"", up.""Address"",
            poi.""OrderItemId"", poi.""Quantity"", poi.""UnitPrice"",
            p.""PizzaId"", p.""Name"", p.""Size"", p.""Price"", p.""IsVegetarian""
        FROM ""PizzaOrders""  po
        JOIN ""Users"" u ON po.""UserId"" = u.""Id""
        LEFT JOIN ""UserProfiles"" up ON u.""Id"" = up.""UserId""
        JOIN ""PizzaOrderItems"" poi ON po.""OrderId"" = poi.""OrderId""
        JOIN ""PizzaItems"" p ON poi.""PizzaId"" = p.""PizzaId""
        ORDER BY po.""OrderDate"", po.""OrderId"";";

            await using var command = new NpgsqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var orderId = reader.GetGuid(0);

                // Check if the order is already added to the list
                var order = orders.Find(o => o.OrderId == orderId);
                if (order == null)
                {
                    // Create new order with user and profile details
                    order = new PizzaOrder
                    {
                        OrderId = orderId,
                        OrderDate = reader.GetDateTime(1),
                        UserId = reader.GetGuid(2),
                        User = new User
                        {
                            Id = reader.GetGuid(2),
                            Name = reader.GetString(3),
                            Email = reader.GetString(4),
                            Age = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                            Profile = new UserProfile
                            {
                                PhoneNumber = reader.IsDBNull(6) ? null : reader.GetString(6),
                                Address = reader.IsDBNull(7) ? null : reader.GetString(7)
                            }
                        },
                        Items = new List<PizzaOrderItem>()
                    };

                    orders.Add(order);
                }

                // Add each order item with pizza details
                var item = new PizzaOrderItem
                {
                    OrderItemId = reader.GetGuid(8),
                    Quantity = reader.GetInt32(9),
                    UnitPrice = reader.GetDecimal(10),
                    Pizza = new PizzaItem
                    {
                        PizzaId = reader.GetGuid(11),
                        Name = reader.GetString(12),
                        Size = reader.IsDBNull(13) ? null : reader.GetString(13),
                        Price = reader.GetDecimal(14),
                        IsVegetarian = reader.GetBoolean(15)
                    }
                };

                order.Items.Add(item);
            }

            return orders;
        }

        /// <summary>
        /// Creates a new pizza order along with its order items in a transaction.
        /// </summary>
        /// <param name="order">The pizza order to create.</param>
        /// <returns>The unique identifier of the created pizza order.</returns>
        /// <exception cref="Exception">Throws if any database operation fails.</exception>
        public async Task<Guid> CreateAsync(PizzaOrder order)
        {
            order.OrderId = Guid.NewGuid();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // Insert main order record
                var insertOrderCmd = new NpgsqlCommand(@"
                    INSERT INTO ""PizzaOrders"" (""OrderId"", ""UserId"", ""OrderDate"")
                    VALUES (@OrderId, @UserId, @OrderDate)", connection, transaction);

                insertOrderCmd.Parameters.AddWithValue("OrderId", order.OrderId);
                insertOrderCmd.Parameters.AddWithValue("UserId", order.UserId);
                insertOrderCmd.Parameters.AddWithValue("OrderDate", order.OrderDate);

                await insertOrderCmd.ExecuteNonQueryAsync();

                // Insert each order item record
                foreach (var item in order.Items)
                {
                    item.OrderItemId = Guid.NewGuid();

                    var insertItemCmd = new NpgsqlCommand(@"
                        INSERT INTO ""PizzaOrderItems"" (""OrderItemId"", ""OrderId"", ""PizzaId"", ""Quantity"", ""UnitPrice"")
                        VALUES (@OrderItemId, @OrderId, @PizzaId, @Quantity, @UnitPrice)", connection, transaction);

                    insertItemCmd.Parameters.AddWithValue("OrderItemId", item.OrderItemId);
                    insertItemCmd.Parameters.AddWithValue("OrderId", order.OrderId);
                    insertItemCmd.Parameters.AddWithValue("PizzaId", item.PizzaId);
                    insertItemCmd.Parameters.AddWithValue("Quantity", item.Quantity);
                    insertItemCmd.Parameters.AddWithValue("UnitPrice", item.UnitPrice);

                    await insertItemCmd.ExecuteNonQueryAsync();
                }

                // Commit transaction if all inserts succeed
                await transaction.CommitAsync();
                return order.OrderId;
            }
            catch (Exception ex)
            {
                // Log error and rollback transaction on failure
                _logger.LogError(ex, "Error creating pizza order");
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Deletes a pizza order and its related order items within a transaction.
        /// </summary>
        /// <param name="orderId">The unique identifier of the pizza order to delete.</param>
        /// <returns><c>true</c> if the order was deleted; otherwise, <c>false</c>.</returns>
        /// <exception cref="Exception">Throws if any database operation fails.</exception>
        public async Task<bool> DeleteAsync(Guid orderId)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // Delete order items first due to foreign key constraints
                var deleteItemsCmd = new NpgsqlCommand(
                    @"DELETE FROM ""PizzaOrderItems"" WHERE ""OrderId"" = @OrderId", connection, transaction);
                deleteItemsCmd.Parameters.AddWithValue("OrderId", orderId);
                await deleteItemsCmd.ExecuteNonQueryAsync();

                // Delete the main order record
                var deleteOrderCmd = new NpgsqlCommand(
                    @"DELETE FROM ""PizzaOrders""  WHERE ""OrderId"" = @OrderId", connection, transaction);
                deleteOrderCmd.Parameters.AddWithValue("OrderId", orderId);
                int affected = await deleteOrderCmd.ExecuteNonQueryAsync();

                // Commit transaction if successful
                await transaction.CommitAsync();

                return affected > 0;
            }
            catch (Exception ex)
            {
                // Log error and rollback transaction on failure
                _logger.LogError(ex, "Error deleting pizza order");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
