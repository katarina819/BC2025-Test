using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BootcampApp.Model;

public interface INotificationRepository
{
    Task<List<Notification>> GetNotificationsByUserIdAsync(Guid userId);
    Task AddNotificationAsync(Notification notification);
    Task MarkAsReadAsync(Guid notificationId);

    // Preporučena proširenja:
    Task MarkAllAsReadAsync(Guid userId);
    Task DeleteNotificationAsync(Guid notificationId);
    Task AddAsync(Notification notification);
    Task<List<Notification>> GetByUserIdAsync(Guid userId);
    

}
