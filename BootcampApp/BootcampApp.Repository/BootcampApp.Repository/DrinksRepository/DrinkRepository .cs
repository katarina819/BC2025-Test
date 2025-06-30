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
    public class DrinkRepository : IDrinkRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<DrinkRepository> _logger;

        public DrinkRepository(string connectionString, ILogger<DrinkRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

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

            // Ovdje vraćamo null ako nije pronađen nijedan redak:
            return null;
        }


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

            return drinks;  // VAŽNO: uvijek moraš vratiti listu
        }

    }

}
