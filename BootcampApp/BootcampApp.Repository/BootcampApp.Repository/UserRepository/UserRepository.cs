using System.Text;
using BootcampApp.Model;
using Microsoft.Extensions.Logging;
using Npgsql;
using static System.Runtime.InteropServices.JavaScript.JSType;



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

        public async Task<Guid> CreateAsync(User user)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            using var trans = await conn.BeginTransactionAsync();

            try
            {
                // Insert into Users table
                var userInsertCmd = new NpgsqlCommand(@"
                INSERT INTO ""Users"" (""Id"", ""Name"", ""Email"", ""Age"")
                VALUES (@Id, @Name, @Email, @Age)", conn);
                userInsertCmd.Parameters.AddWithValue("Id", user.Id);
                userInsertCmd.Parameters.AddWithValue("Name", user.Name);
                userInsertCmd.Parameters.AddWithValue("Email", user.Email);
                userInsertCmd.Parameters.AddWithValue("Age", (object?)user.Age ?? DBNull.Value);

                await userInsertCmd.ExecuteNonQueryAsync();

                // Insert into UserProfiles table
                if (user.Profile is not null)
                {
                    var profileInsertCmd = new NpgsqlCommand(@"
                    INSERT INTO ""UserProfiles"" (""UserId"", ""PhoneNumber"", ""Address"")
                    VALUES (@UserId, @PhoneNumber, @Address)", conn);

                    profileInsertCmd.Parameters.AddWithValue("UserId", user.Id);
                    profileInsertCmd.Parameters.AddWithValue("PhoneNumber", (object?)user.Profile.PhoneNumber ?? DBNull.Value);
                    profileInsertCmd.Parameters.AddWithValue("Address", (object?)user.Profile.Address ?? DBNull.Value);

                    await profileInsertCmd.ExecuteNonQueryAsync();
                }

                await trans.CommitAsync();
                return user.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                await trans.RollbackAsync();
                throw;
            }
        }


        public async Task<bool> UpdateAsync(Guid id, User updatedUser)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();


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

        public async Task<List<User>> GetUsersPagedAsync(int page, int pageSize)
        {
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            var users = new List<User>();
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sb = new StringBuilder(@"
        SELECT ""Id"", ""Name"", ""Email"", ""Age""
        FROM ""Users""
        ORDER BY ""Name"" ASC
        OFFSET @offset LIMIT @limit
    ");

            using var cmd = new NpgsqlCommand(sb.ToString(), conn);
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
                });
            }

            return users;
        }



        public async Task<int> CountFilteredUsersAsync(string? searchValue)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sb = new StringBuilder("SELECT COUNT(*) FROM \"Users\" WHERE 1=1");

            if (!string.IsNullOrWhiteSpace(searchValue))
                sb.Append(" AND (\"Name\" ILIKE @search OR \"Email\" ILIKE @search)");

            using var cmd = new NpgsqlCommand(sb.ToString(), conn);
            if (!string.IsNullOrWhiteSpace(searchValue))
                cmd.Parameters.AddWithValue("search", $"%{searchValue}%");

            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task<int> CountUsersAsync()
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = "SELECT COUNT(*) FROM \"Users\"";

            using var cmd = new NpgsqlCommand(sql, conn);
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task<List<User>> SearchAsync(int page, int pageSize)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sb = new StringBuilder("SELECT \"Id\", \"Name\", \"Email\" FROM \"Users\" ORDER BY \"Name\" ASC");

            int offset = (page < 1 ? 0 : (page - 1) * pageSize);
            sb.Append(" LIMIT @pageSize OFFSET @offset");

            using var cmd = new NpgsqlCommand(sb.ToString(), conn);
            cmd.Parameters.AddWithValue("pageSize", pageSize);
            cmd.Parameters.AddWithValue("offset", offset);

            var users = new List<User>();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    Id = reader.GetGuid(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Email = reader.GetString(reader.GetOrdinal("Email"))
                });
            }

            return users;
        }

        public async Task<User?> GetByNameAndEmailAsync(string name, string email)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = @"
            SELECT u.""Id"", u.""Name"", u.""Email"", u.""Age"",
                   up.""UserId"", up.""PhoneNumber"", up.""Address""
            FROM ""Users"" u
            LEFT JOIN ""UserProfiles"" up ON u.""Id"" = up.""UserId""
            WHERE u.""Name"" = @Name AND u.""Email"" = @Email
            LIMIT 1";

            using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("Name", name);
            cmd.Parameters.AddWithValue("Email", email);

            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            // Čitanje podataka iz readera i mapiranje na objekt User i UserProfile
            var user = new User
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                Name = reader.GetString(reader.GetOrdinal("Name")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Age = reader.IsDBNull(reader.GetOrdinal("Age")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Age")),
                Profile = new UserProfile
                {
                    UserId = reader.IsDBNull(reader.GetOrdinal("UserId")) ? (Guid?)null : reader.GetGuid(reader.GetOrdinal("UserId")),
                    PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                    Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
                }
            };

            return user;
        }

        public async Task<User?> GetUserWithProfileByIdAsync(Guid id)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"
        SELECT u.id, u.name, u.email, u.age,
               p.phone_number, p.address
        FROM users u
        LEFT JOIN profiles p ON u.id = p.user_id
        WHERE u.id = @id
    ";

            await using var cmd = new NpgsqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("id", id);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            var user = new User
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Email = reader.GetString(2),
                Age = reader.GetInt32(3),
                Profile = new UserProfile
                {
                    PhoneNumber = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Address = reader.IsDBNull(5) ? null : reader.GetString(5)
                }
            };

            return user;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var sql = @"
        SELECT u.""Id"", u.""Name"", u.""Email"", u.""Age"",
               up.""UserId"", up.""PhoneNumber"", up.""Address""
        FROM ""Users"" u
        LEFT JOIN ""UserProfiles"" up ON u.""Id"" = up.""UserId""
        WHERE u.""Username"" = @username
        LIMIT 1";

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("username", username);

            await using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

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

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            const string sql = "SELECT id, name, email, age, password_hash FROM users WHERE name = @name";

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("name", username);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new User
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Email = reader.GetString(2),
                    Age = reader.GetInt32(3),
                    
                };
            }

            return null;
        }














    }


}


    



