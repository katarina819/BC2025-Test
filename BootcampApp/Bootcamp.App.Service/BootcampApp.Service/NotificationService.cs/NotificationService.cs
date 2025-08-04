using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;
using BootcampApp.Repository;
using BootcampApp.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace BootcampApp.Service
{
    /// <summary>
    /// Provides notification-related services, such as retrieving, adding, marking, and deleting notifications.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly string _connectionString;
        private readonly ILogger<NotificationService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository for user data access.</param>
        /// <param name="repository">The notification repository for notification data access.</param>
        public NotificationService(IUserRepository userRepository, INotificationRepository notificationRepository, IHubContext<NotificationHub> hubContext, string connectionString, ILogger<NotificationService> logger)
        {
            _userRepository = userRepository;
            _notificationRepository = notificationRepository;
            _hubContext = hubContext;
            _connectionString = connectionString;
            _logger = logger;
        }

        /// <summary>
        /// Gets all notifications for a specific user asynchronously.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        /// <returns>A list of <see cref="Notification"/> objects for the user.</returns>
        public Task<List<Notification>> GetNotificationsByUserIdAsync(Guid userId)
        {
            return _notificationRepository.GetNotificationsByUserIdAsync(userId);
        }

        /// <summary>
        /// Adds a new notification asynchronously.
        /// </summary>
        /// <param name="notification">The notification to add.</param>
        public Task AddNotificationAsync(Notification notification)
        {
            return _notificationRepository.AddNotificationAsync(notification);
        }

        /// <summary>
        /// Marks a specific notification as read asynchronously.
        /// </summary>
        /// <param name="notificationId">The notification's unique identifier.</param>
        public Task MarkAsReadAsync(Guid notificationId)
        {
            return _notificationRepository.MarkAsReadAsync(notificationId);
        }

        /// <summary>
        /// Marks all notifications for a specific user as read asynchronously.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        public async Task MarkAllAsReadAsync(Guid userId)
        {
            var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId);
            foreach (var notification in notifications)
            {
                if (!notification.IsRead)
                    await _notificationRepository.MarkAsReadAsync(notification.NotificationId);
            }
        }

        /// <summary>
        /// Deletes a specific notification asynchronously.
        /// Note: Implementation depends on the repository supporting delete operation.
        /// </summary>
        /// <param name="notificationId">The notification's unique identifier.</param>
        public async Task DeleteNotificationAsync(Guid notificationId)
        {
            // Assume DeleteAsync exists in the repository; implement if available.
            // await _repository.DeleteAsync(notificationId);
        }

        /// <summary>
        /// Creates a new notification for a user with an optional link and adds it asynchronously.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        /// <param name="message">The notification message.</param>
        /// <param name="link">An optional URL link related to the notification.</param>
        public async Task CreateNotificationAsync(Guid userId, string message, string? link = null)
        {
            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                UserId = userId,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.UtcNow,
                Link = link
            };

            await _notificationRepository.AddNotificationAsync(notification);
            _logger.LogInformation($"[SignalR] Sending notification to user {userId}: {message}");

            try
            {
                // SignalR payload – neka odgovara Notification modelu na frontend strani
                await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", new
                {
                    notificationId = notification.NotificationId,
                    userId = userId,
                    message = notification.Message,
                    isRead = notification.IsRead,
                    createdAt = notification.CreatedAt,
                    link = notification.Link
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[SignalR] Failed to send notification to user {userId}");
            }
        }

        /// <summary>
        /// Sends a notification to a user specified by username with an optional link.
        /// </summary>
        /// <param name="username">The username of the recipient user.</param>
        /// <param name="message">The notification message.</param>
        /// <param name="link">An optional URL link related to the notification.</param>
        /// <exception cref="Exception">Thrown if the user is not found.</exception>
        public async Task SendNotificationToUserAsync(string username, string message, string? link = null)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user == null)
                throw new Exception("User not found");

            var notification = new Notification
            {
                UserId = user.Id,
                Message = message,
                Link = link,
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification);
        }

        public async Task<bool> UpdateNotificationStatusAsync(Guid userId, Guid notificationId, bool isRead)
        {
            await using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            var sql = "UPDATE notifications SET is_read = @isRead WHERE notification_id = @notificationId AND user_id = @userId";

            await using var command = new NpgsqlCommand(sql, connection);
            command.Parameters.AddWithValue("isRead", isRead);
            command.Parameters.AddWithValue("notificationId", notificationId);
            command.Parameters.AddWithValue("userId", userId);

            var rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task ClearNotificationsAsync(Guid userId)
        {
            await _notificationRepository.SoftDeleteAllByUserIdAsync(userId);
        }


        public async Task<IEnumerable<Notification>> GetActiveNotificationsAsync(Guid userId)
        {
            var notifications = await _notificationRepository.GetAllByUserIdAsync(userId);
            return notifications.Where(n => !n.IsDeleted);
        }

        public async Task DeleteAllNotificationsForUserAsync(Guid userId)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            var command = new NpgsqlCommand(
                "DELETE FROM notifications WHERE user_id = @userId", conn);

            command.Parameters.AddWithValue("userId", userId);

            await command.ExecuteNonQueryAsync();
        }








    }
}
