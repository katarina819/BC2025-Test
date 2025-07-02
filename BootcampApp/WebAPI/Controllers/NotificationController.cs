using BootcampApp.Model;
using BootcampApp.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Controller for managing user notifications.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationController"/> class.
        /// </summary>
        /// <param name="notificationService">Service to manage notifications.</param>
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Gets all notifications for a specified user.
        /// </summary>
        /// <param name="userId">User's unique identifier.</param>
        /// <returns>List of notifications or 404 if none found.</returns>
        [HttpGet("users/{userId}/notifications")]
        public async Task<IActionResult> GetUserNotifications(Guid userId)
        {
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);
            if (notifications == null)
                return NotFound();
            return Ok(notifications);
        }

        /// <summary>
        /// Adds a new notification.
        /// </summary>
        /// <param name="notification">Notification object to add.</param>
        /// <returns>Created notification with location header.</returns>
        [HttpPost]
        public async Task<ActionResult> AddNotification([FromBody] Notification notification)
        {
            if (notification == null)
                return BadRequest("Notification is null.");

            notification.NotificationId = Guid.NewGuid();
            notification.CreatedAt = DateTime.UtcNow;
            await _notificationService.AddNotificationAsync(notification);

            return CreatedAtAction(nameof(GetUserNotifications), new { userId = notification.UserId }, notification);
        }

        /// <summary>
        /// Marks a specific notification as read.
        /// </summary>
        /// <param name="id">Notification ID.</param>
        /// <returns>No content on success.</returns>
        [HttpPut("mark-as-read/{id}")]
        public async Task<ActionResult> MarkAsRead(Guid id)
        {
            await _notificationService.MarkAsReadAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Marks all notifications for a user as read.
        /// </summary>
        /// <param name="userId">User ID.</param>
        /// <returns>No content on success.</returns>
        [HttpPut("mark-all-as-read/{userId}")]
        public async Task<ActionResult> MarkAllAsRead(Guid userId)
        {
            await _notificationService.MarkAllAsReadAsync(userId);
            return NoContent();
        }

        /// <summary>
        /// Deletes a notification by its ID.
        /// </summary>
        /// <param name="id">Notification ID.</param>
        /// <returns>No content on success.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _notificationService.DeleteNotificationAsync(id);
            return NoContent();
        }
    }
}
