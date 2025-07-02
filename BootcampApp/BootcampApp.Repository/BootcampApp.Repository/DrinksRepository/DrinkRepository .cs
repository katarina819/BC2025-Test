using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BootcampApp.Model;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace BootcampApp.Repository.BootcampApp.Repository.DrinksRepository
{
    /// <summary>
    /// Repository class for managing Drink entities in the database.
    /// </summary>
    public class DrinkRepository : IDrinkRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<DrinkRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DrinkRepository"/> class.
        /// </summary>
        /// <param name="connectionString">Database connection string.</param>
        /// <param name="logger">Logger instance for logging information and errors.</param>
        public DrinkRepository(string connectionString, ILogger<DrinkRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a single Drink by its unique identifier asynchronously.
        /// </summary>
        /// <param name="drinkId">The unique identifier of the Drink to retrieve.</param>
        /// <returns>
        /// A <see cref="Drink"/> object if found; otherwise, <c>null</c>.
        /// </returns>
        public async Task<Drink?> GetByIdAsync(Guid drinkId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"SELECT ""DrinkId"", ""Name"", ""Size"", ""Price""
                           FROM ""Drinks""
                           WHERE ""DrinkId"" = @DrinkId";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("DrinkId", drinkId);

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Drink
                {
                    DrinkId = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Size = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Price = reader.GetDecimal(3)
                };
            }

            // Returns null if no record is found
            return null;
        }

        /// <summary>
        /// Retrieves all Drinks asynchronously from the database.
        /// </summary>
        /// <returns>A list of all <see cref="Drink"/> entities.</returns>
        public async Task<List<Drink>> GetAllAsync()
        {
            var drinks = new List<Drink>();

            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"SELECT ""DrinkId"", ""Name"", ""Size"", ""Price"" FROM ""Drinks""";

            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                drinks.Add(new Drink
                {
                    DrinkId = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Size = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Price = reader.GetDecimal(3)
                });
            }

            // Important: Always return the list, even if empty
            return drinks;
        }
    }
}
