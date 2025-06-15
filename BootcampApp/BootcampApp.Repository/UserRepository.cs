using BootcampApp.Model;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace BootcampApp.Repository
{
    public class UserRepository : IUserRepository

    {
        private readonly string _connectionString;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(string connectionString, ILogger<UserRepository> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users = new List<User>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("SELECT \"Id\", \"Name\", \"Email\" FROM \"Users\"", conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2)
                });
            }

            return users;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand(@"
                SELECT u.""Id"", u.""Name"", u.""Email"", u.""Age"",
                       up.""UserId"", up.""PhoneNumber"", up.""Address""
                FROM ""Users"" u
                LEFT JOIN ""UserProfiles"" up ON u.""Id"" = up.""UserId""
                WHERE u.""Id"" = @id", conn);

            cmd.Parameters.AddWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    Age = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    Profile = new UserProfile
                    {
                        UserId = reader.IsDBNull(4) ? Guid.Empty : reader.GetGuid(4),
                        PhoneNumber = reader.IsDBNull(5) ? null : reader.GetString(5),
                        Address = reader.IsDBNull(6) ? null : reader.GetString(6),
                    }
                };
            }

            return null;
        }

        public async Task<Guid> CreateAsync(User newUser)
        {
            newUser.Id = Guid.NewGuid();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            // Transakcija za konzistentnost upisa
            await using var transaction = await conn.BeginTransactionAsync();

            try
            {
                var cmdUser = new NpgsqlCommand(
                    "INSERT INTO \"Users\" (\"Id\", \"Name\", \"Email\", \"Age\") VALUES (@id, @name, @email, @age)", conn, transaction);
                cmdUser.Parameters.AddWithValue("id", newUser.Id);
                cmdUser.Parameters.AddWithValue("name", newUser.Name);
                cmdUser.Parameters.AddWithValue("email", newUser.Email);
                cmdUser.Parameters.AddWithValue("age", (object?)newUser.Age ?? DBNull.Value);

                await cmdUser.ExecuteNonQueryAsync();

                if (newUser.Profile != null)
                {
                    var cmdProfile = new NpgsqlCommand(
                        "INSERT INTO \"UserProfiles\" (\"UserId\", \"PhoneNumber\", \"Address\") VALUES (@userId, @phone, @address)", conn, transaction);
                    cmdProfile.Parameters.AddWithValue("userId", newUser.Id);
                    cmdProfile.Parameters.AddWithValue("phone", (object?)newUser.Profile.PhoneNumber ?? DBNull.Value);
                    cmdProfile.Parameters.AddWithValue("address", (object?)newUser.Profile.Address ?? DBNull.Value);

                    await cmdProfile.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
                return newUser.Id;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<bool> UpdateAsync(Guid id, User updatedUser)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            // Transakcija za konzistentno ažuriranje više tabela
            await using var transaction = await conn.BeginTransactionAsync();

            try
            {
                var cmd = new NpgsqlCommand(
                    "UPDATE \"Users\" SET \"Name\" = @name, \"Email\" = @email, \"Age\" = @age WHERE \"Id\" = @id", conn, transaction);
                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("name", updatedUser.Name);
                cmd.Parameters.AddWithValue("email", updatedUser.Email);
                cmd.Parameters.AddWithValue("age", (object?)updatedUser.Age ?? DBNull.Value);

                var rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (updatedUser.Profile != null)
                {
                    var cmdProfile = new NpgsqlCommand(@"
                INSERT INTO ""UserProfiles"" (""UserId"", ""PhoneNumber"", ""Address"")
                VALUES (@userId, @phone, @address)
                ON CONFLICT (""UserId"") DO UPDATE
                SET ""PhoneNumber"" = EXCLUDED.""PhoneNumber"",
                    ""Address"" = EXCLUDED.""Address""
            ", conn, transaction);

                    cmdProfile.Parameters.AddWithValue("userId", id);
                    cmdProfile.Parameters.AddWithValue("phone", (object?)updatedUser.Profile.PhoneNumber ?? DBNull.Value);
                    cmdProfile.Parameters.AddWithValue("address", (object?)updatedUser.Profile.Address ?? DBNull.Value);

                    await cmdProfile.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
                return rowsAffected > 0;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


        public async Task<bool> DeleteAsync(Guid id)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new NpgsqlCommand("DELETE FROM \"Users\" WHERE \"Id\" = @id", conn);
            cmd.Parameters.AddWithValue("id", id);

            var rowsAffected = await cmd.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<List<User>> SearchAsync(string? searchValue, string? sortBy, int page = 1, int pageSize = 10)
        {
            var users = new List<User>();
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sb = new System.Text.StringBuilder(@"
                SELECT u.""Id"", u.""Name"", u.""Email"", u.""Age"",
                       up.""UserId"", up.""PhoneNumber"", up.""Address""
                FROM ""Users"" u
                LEFT JOIN ""UserProfiles"" up ON u.""Id"" = up.""UserId""
                WHERE 1=1
            ");

            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                sb.Append(" AND (u.\"Name\" ILIKE @search OR u.\"Email\" ILIKE @search)");
            }

            string sortColumn = sortBy?.ToLower() switch
            {
                "email" => "u.\"Email\"",
                "name" => "u.\"Name\"",
                "age" => "u.\"Age\"",
                _ => "u.\"Name\""
            };

            sb.Append($" ORDER BY {sortColumn}");
            sb.Append(" OFFSET @offset LIMIT @limit");

            var cmd = new NpgsqlCommand(sb.ToString(), conn);

            
            

            if (!string.IsNullOrWhiteSpace(searchValue))
                cmd.Parameters.AddWithValue("search", $"%{searchValue}%");

            cmd.Parameters.AddWithValue("offset", (page - 1) * pageSize);
            cmd.Parameters.AddWithValue("limit", pageSize);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    Age = reader.IsDBNull(3) ? null : reader.GetInt32(3),
                    Profile = new UserProfile
                    {
                        UserId = reader.IsDBNull(4) ? Guid.Empty : reader.GetGuid(4),
                        PhoneNumber = reader.IsDBNull(5) ? null : reader.GetString(5),
                        Address = reader.IsDBNull(6) ? null : reader.GetString(6)
                    }
                });
            }

            return users;
        }

        public async Task<IEnumerable<User>> GetUsersPagedAsync(int page, int rpp)
        {
            var users = new List<User>();
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = @"SELECT * FROM ""Users"" ORDER BY ""Name"" LIMIT @Limit OFFSET @Offset";
            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("Limit", rpp);
            cmd.Parameters.AddWithValue("Offset", (page - 1) * rpp);

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    Age = reader.IsDBNull(reader.GetOrdinal("Age")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Age"))
                });
            }

            return users;
        }
    }
}

