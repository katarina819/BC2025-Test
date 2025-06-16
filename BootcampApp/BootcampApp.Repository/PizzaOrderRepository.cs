using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace BootcampApp.Repository
{
    public class PizzaOrderRepository : IPizzaOrderRepository
    {
        private readonly ILogger<PizzaOrderRepository> _logger;
        private readonly string _connectionString;

        public PizzaOrderRepository(string connectionString, ILogger<PizzaOrderRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<PizzaOrder?> GetByIdAsync(Guid orderId)
        {
            PizzaOrder? order = null;

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

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
                    return null;
                }
            }

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
                        Pizza = null!
                    };

                    order.Items.Add(item);
                }
            }

            return order;
        }

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
        FROM ""PizzaOrders"" po
        JOIN ""Users"" u ON po.""UserId"" = u.""Id""
        LEFT JOIN ""UserProfiles"" up ON u.""Id"" = up.""UserId""
        JOIN ""PizzaOrderItems"" poi ON po.""OrderId"" = poi.""OrderId""
        JOIN ""PizzaItems"" p ON poi.""PizzaId"" = p.""PizzaId""
        ORDER BY po.""OrderDate"", po.""OrderId"";
    ";

            await using var command = new NpgsqlCommand(sql, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var orderId = reader.GetGuid(0);

                var order = orders.Find(o => o.OrderId == orderId);
                if (order == null)
                {
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




        public async Task<Guid> CreateAsync(PizzaOrder order)
        {
            order.OrderId = Guid.NewGuid();

            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var insertOrderCmd = new NpgsqlCommand(@"
                    INSERT INTO ""PizzaOrders"" (""OrderId"", ""UserId"", ""OrderDate"")
                    VALUES (@OrderId, @UserId, @OrderDate)", connection, transaction);

                insertOrderCmd.Parameters.AddWithValue("OrderId", order.OrderId);
                insertOrderCmd.Parameters.AddWithValue("UserId", order.UserId);
                insertOrderCmd.Parameters.AddWithValue("OrderDate", order.OrderDate);

                await insertOrderCmd.ExecuteNonQueryAsync();

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

                await transaction.CommitAsync();
                return order.OrderId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating pizza order");
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Guid orderId)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();
            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                var deleteItemsCmd = new NpgsqlCommand(
                    @"DELETE FROM ""PizzaOrderItems"" WHERE ""OrderId"" = @OrderId", connection, transaction);
                deleteItemsCmd.Parameters.AddWithValue("OrderId", orderId);
                await deleteItemsCmd.ExecuteNonQueryAsync();

                var deleteOrderCmd = new NpgsqlCommand(
                    @"DELETE FROM ""PizzaOrders"" WHERE ""OrderId"" = @OrderId", connection, transaction);
                deleteOrderCmd.Parameters.AddWithValue("OrderId", orderId);
                int affected = await deleteOrderCmd.ExecuteNonQueryAsync();

                await transaction.CommitAsync();

                return affected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting pizza order");
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
