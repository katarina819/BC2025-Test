using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;

namespace BootcampApp.Service
{
    /// <summary>
    /// Provides notification-related operations for users.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Retrieves all notifications for a specific user asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A list of <see cref="Notification"/> objects for the specified user.</returns>
        Task<List<Notification>> GetNotificationsByUserIdAsync(Guid userId);

        /// <summary>
        /// Adds a new notification asynchronously.
        /// </summary>
        /// <param name="notification">The notification to add.</param>
        Task AddNotificationAsync(Notification notification);

        /// <summary>
        /// Marks a specific notification as read asynchronously.
        /// </summary>
        /// <param name="notificationId">The unique identifier of the notification to mark as read.</param>
        Task MarkAsReadAsync(Guid notificationId);

        /// <summary>
        /// Marks all notifications for a specific user as read asynchronously.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose notifications will be marked as read.</param>
        Task MarkAllAsReadAsync(Guid userId);

        /// <summary>
        /// Deletes a specific notification asynchronously.
        /// </summary>
        /// <param name="notificationId">The unique identifier of the notification to delete.</param>
        Task DeleteNotificationAsync(Guid notificationId);

        /// <summary>
        /// Creates and adds a new notification asynchronously for a specific user with an optional link.
        /// </summary>
        /// <param name="userId">The unique identifier of the user to notify.</param>
        /// <param name="message">The notification message.</param>
        /// <param name="link">An optional link related to the notification.</param>
        Task CreateNotificationAsync(Guid userId, string message, string? link = null);
    }
}
