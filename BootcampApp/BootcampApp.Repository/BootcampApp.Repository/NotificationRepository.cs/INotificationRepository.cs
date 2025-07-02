using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;

/// <summary>
/// Defines operations for managing user notifications.
/// </summary>
public interface INotificationRepository
{
    /// <summary>
    /// Retrieves all notifications associated with a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of <see cref="Notification"/> objects.</returns>
    Task<List<Notification>> GetNotificationsByUserIdAsync(Guid userId);

    /// <summary>
    /// Adds a new notification to the system.
    /// </summary>
    /// <param name="notification">The <see cref="Notification"/> to be added.</param>
    Task AddNotificationAsync(Notification notification);

    /// <summary>
    /// Marks a specific notification as read.
    /// </summary>
    /// <param name="notificationId">The unique identifier of the notification to mark as read.</param>
    Task MarkAsReadAsync(Guid notificationId);

    /// <summary>
    /// Marks all notifications for a specific user as read.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    Task MarkAllAsReadAsync(Guid userId);

    /// <summary>
    /// Deletes a specific notification.
    /// </summary>
    /// <param name="notificationId">The unique identifier of the notification to delete.</param>
    Task DeleteNotificationAsync(Guid notificationId);

    /// <summary>
    /// Adds a new notification (alternative/additional method).
    /// </summary>
    /// <param name="notification">The <see cref="Notification"/> to add.</param>
    Task AddAsync(Notification notification);

    /// <summary>
    /// Retrieves all notifications for a specific user (alternative method).
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A list of <see cref="Notification"/> objects.</returns>
    Task<List<Notification>> GetByUserIdAsync(Guid userId);
}
