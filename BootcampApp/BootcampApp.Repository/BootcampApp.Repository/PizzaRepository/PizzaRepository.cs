using System;
using System.Threading.Tasks;
using BootcampApp.Model;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace BootcampApp.Repository
{
    /// <summary>
    /// Repository class responsible for CRUD operations on PizzaItem entities.
    /// Communicates with PostgreSQL database using Npgsql.
    /// </summary>
    public class PizzaRepository : IPizzaRepository
    {
        private readonly ILogger<PizzaRepository> _logger;
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="PizzaRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The PostgreSQL connection string.</param>
        /// <param name="logger">The logger instance for logging errors and info.</param>
        public PizzaRepository(string connectionString, ILogger<PizzaRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a PizzaItem by its unique identifier asynchronously.
        /// </summary>
        /// <param name="pizzaId">The unique identifier of the pizza item.</param>
        /// <returns>
        /// A <see cref="PizzaItem"/> if found; otherwise, <c>null</c>.
        /// </returns>
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

        /// <summary>
        /// Retrieves all PizzaItem records asynchronously.
        /// </summary>
        /// <returns>A list containing all pizza items.</returns>
        public async Task<List<PizzaItem>> GetAllAsync()
        {
            var pizzas = new List<PizzaItem>();

            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"SELECT ""PizzaId"", ""Name"", ""Size"", ""Price"", ""IsVegetarian""
                           FROM ""PizzaItems""";

            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                pizzas.Add(new PizzaItem
                {
                    PizzaId = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Size = reader.GetString(2),
                    Price = reader.GetDecimal(3),
                    IsVegetarian = reader.GetBoolean(4)
                });
            }

            return pizzas;
        }
    }
}
