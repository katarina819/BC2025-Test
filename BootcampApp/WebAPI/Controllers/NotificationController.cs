using BootcampApp.Model;
using BootcampApp.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;



namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET: api/notification/user/{userId}
        [HttpGet("users/{userId}/notifications")]
        public async Task<IActionResult> GetUserNotifications(Guid userId)
        {
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);
            if (notifications == null)
                return NotFound();
            return Ok(notifications);
        }




        // POST: api/notification
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

        // PUT: api/notification/mark-as-read/{id}
        [HttpPut("mark-as-read/{id}")]
        public async Task<ActionResult> MarkAsRead(Guid id)
        {
            await _notificationService.MarkAsReadAsync(id);
            return NoContent();
        }

        // PUT: api/notification/mark-all-as-read/{userId}
        [HttpPut("mark-all-as-read/{userId}")]
        public async Task<ActionResult> MarkAllAsRead(Guid userId)
        {
            await _notificationService.MarkAllAsReadAsync(userId);
            return NoContent();
        }

        // DELETE: api/notification/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _notificationService.DeleteNotificationAsync(id);
            return NoContent();
        }

    }

}
