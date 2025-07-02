using System;

namespace BootcampApp.Model
{
    /// <summary>
    /// Represents a notification sent to a user.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Gets or sets the unique identifier of the notification.
        /// </summary>
        public Guid NotificationId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the user who will receive the notification.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets the content message of the notification.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the notification has been read.
        /// </summary>
        public bool IsRead { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the notification was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets an optional link associated with the notification.
        /// </summary>
        public string? Link { get; set; }
    }
}
