using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;
using Npgsql;

namespace BootcampApp.Repository
{
    /// <summary>
    /// Provides methods for managing user notifications using a PostgreSQL database.
    /// </summary>
    public class NotificationRepository : INotificationRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string for the PostgreSQL database.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="connectionString"/> is null.</exception>
        public NotificationRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Retrieves all notifications for a specific user, ordered by creation time (descending).
        /// </summary>
        /// <param name="userId">The ID of the user whose notifications to retrieve.</param>
        /// <returns>A list of <see cref="Notification"/> objects.</returns>
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

        /// <summary>
        /// Adds a new notification by delegating to the <see cref="AddAsync"/> method.
        /// </summary>
        /// <param name="notification">The <see cref="Notification"/> to add.</param>
        public async Task AddNotificationAsync(Notification notification)
        {
            await AddAsync(notification);
        }

        /// <summary>
        /// Adds a new notification to the database.
        /// </summary>
        /// <param name="notification">The <see cref="Notification"/> to add.</param>
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

        /// <summary>
        /// Marks all notifications for a specific user as read.
        /// </summary>
        /// <param name="userId">The ID of the user whose notifications should be marked as read.</param>
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

        /// <summary>
        /// Deletes a specific notification by its ID.
        /// </summary>
        /// <param name="notificationId">The ID of the notification to delete.</param>
        public async Task DeleteNotificationAsync(Guid notificationId)
        {
            const string sql = "DELETE FROM notifications WHERE notification_id = @NotificationId;";

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("NotificationId", notificationId);

            await cmd.ExecuteNonQueryAsync();
        }

        /// <summary>
        /// Marks a specific notification as read.
        /// </summary>
        /// <param name="notificationId">The ID of the notification to mark as read.</param>
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

        /// <summary>
        /// Retrieves all notifications for a specific user.
        /// This method wraps <see cref="GetByUserIdAsync"/> for naming consistency.
        /// </summary>
        /// <param name="userId">The ID of the user whose notifications to retrieve.</param>
        /// <returns>A list of <see cref="Notification"/> objects.</returns>
        public async Task<List<Notification>> GetNotificationsByUserIdAsync(Guid userId)
        {
            return await GetByUserIdAsync(userId);
        }
    }
}
