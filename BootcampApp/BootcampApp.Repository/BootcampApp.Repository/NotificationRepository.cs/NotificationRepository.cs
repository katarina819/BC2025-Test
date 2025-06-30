using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;
using Npgsql;

namespace BootcampApp.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly string _connectionString;

        public NotificationRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<List<Notification>> GetByUserIdAsync(Guid userId)
        {
            var notifications = new List<Notification>();

            const string sql = @"
                SELECT notification_id, user_id, message, is_read, created_at, link
                FROM notifications
                WHERE user_id = @UserId
                ORDER BY created_at DESC;";

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("UserId", userId);

            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                notifications.Add(new Notification
                {
                    NotificationId = reader.GetGuid(0),
                    UserId = reader.GetGuid(1),
                    Message = reader.GetString(2),
                    IsRead = reader.GetBoolean(3),
                    CreatedAt = reader.GetDateTime(4),
                    Link = reader.IsDBNull(5) ? null : reader.GetString(5)
                });
            }

            return notifications;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await AddAsync(notification);
        }

        public async Task AddAsync(Notification notification)
        {
            const string sql = @"
                INSERT INTO notifications (notification_id, user_id, message, is_read, created_at, link)
                VALUES (@notification_id, @user_id, @message, @is_read, @created_at, @link)";

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("notification_id", notification.NotificationId);
            cmd.Parameters.AddWithValue("user_id", notification.UserId);
            cmd.Parameters.AddWithValue("message", notification.Message);
            cmd.Parameters.AddWithValue("is_read", notification.IsRead);
            cmd.Parameters.AddWithValue("created_at", notification.CreatedAt);
            cmd.Parameters.AddWithValue("link", (object?)notification.Link ?? DBNull.Value);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task MarkAllAsReadAsync(Guid userId)
        {
            const string sql = @"
        UPDATE notifications
        SET is_read = TRUE
        WHERE user_id = @user_id";

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("user_id", userId);

            await cmd.ExecuteNonQueryAsync();
        }


        public async Task DeleteNotificationAsync(Guid notificationId)
        {
            const string sql = "DELETE FROM notifications WHERE notification_id = @NotificationId;";

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("NotificationId", notificationId);

            await cmd.ExecuteNonQueryAsync();
        }


        public async Task MarkAsReadAsync(Guid notificationId)
        {
            const string sql = @"
        UPDATE notifications
        SET is_read = TRUE
        WHERE notification_id = @notification_id";

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("notification_id", notificationId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<Notification>> GetNotificationsByUserIdAsync(Guid userId)
        {
            return await GetByUserIdAsync(userId);
        }




    }
}
