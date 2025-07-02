using BootcampApp.Model;
using BootcampApp.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BootcampApp.Service
{
    /// <summary>
    /// Provides notification-related services, such as retrieving, adding, marking, and deleting notifications.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository for user data access.</param>
        /// <param name="repository">The notification repository for notification data access.</param>
        public NotificationService(IUserRepository userRepository, INotificationRepository repository)
        {
            _userRepository = userRepository;
            _repository = repository;
        }

        /// <summary>
        /// Gets all notifications for a specific user asynchronously.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        /// <returns>A list of <see cref="Notification"/> objects for the user.</returns>
        public Task<List<Notification>> GetNotificationsByUserIdAsync(Guid userId)
        {
            return _repository.GetNotificationsByUserIdAsync(userId);
        }

        /// <summary>
        /// Adds a new notification asynchronously.
        /// </summary>
        /// <param name="notification">The notification to add.</param>
        public Task AddNotificationAsync(Notification notification)
        {
            return _repository.AddNotificationAsync(notification);
        }

        /// <summary>
        /// Marks a specific notification as read asynchronously.
        /// </summary>
        /// <param name="notificationId">The notification's unique identifier.</param>
        public Task MarkAsReadAsync(Guid notificationId)
        {
            return _repository.MarkAsReadAsync(notificationId);
        }

        /// <summary>
        /// Marks all notifications for a specific user as read asynchronously.
        /// </summary>
        /// <param name="userId">The user's unique identifier.</param>
        public async Task MarkAllAsReadAsync(Guid userId)
        {
            var notifications = await _repository.GetNotificationsByUserIdAsync(userId);
            foreach (var notification in notifications)
            {
                if (!notification.IsRead)
                    await _repository.MarkAsReadAsync(notification.NotificationId);
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

            await _repository.AddNotificationAsync(notification);
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

            await _repository.AddAsync(notification);
        }
    }
}
