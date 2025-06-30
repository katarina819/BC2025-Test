using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BootcampApp.Model;
using BootcampApp.Service;

namespace BootcampApp.Service
{
    public interface INotificationService
    {
        Task<List<Notification>> GetNotificationsByUserIdAsync(Guid userId);
        Task AddNotificationAsync(Notification notification);
        Task MarkAsReadAsync(Guid notificationId);
        Task MarkAllAsReadAsync(Guid userId);
        Task DeleteNotificationAsync(Guid notificationId);
        Task CreateNotificationAsync(Guid userId, string message, string? link = null);

    }

}
