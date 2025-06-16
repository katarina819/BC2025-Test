using System;
using System.Threading.Tasks;
using BootcampApp.Model;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace BootcampApp.Repository
{
    public class PizzaRepository : IPizzaRepository
    {
        private readonly ILogger<PizzaRepository> _logger;
        private readonly string _connectionString;

        public PizzaRepository(string connectionString, ILogger<PizzaRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<PizzaItem?> GetByIdAsync(Guid pizzaId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"SELECT ""PizzaId"", ""Name"", ""Size"", ""Price"", ""IsVegetarian""
                           FROM ""PizzaItems""
                           WHERE ""PizzaId"" = @PizzaId";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("PizzaId", pizzaId);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new PizzaItem
                {
                    PizzaId = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Size = reader.GetString(2),
                    Price = reader.GetDecimal(3),
                    IsVegetarian = reader.GetBoolean(4)
                };
            }

            return null;
        }
    }
}

