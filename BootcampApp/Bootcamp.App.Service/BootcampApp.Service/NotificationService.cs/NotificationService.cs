using BootcampApp.Model;
using BootcampApp.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BootcampApp.Service
{
    public class NotificationService : INotificationService
    {
        private readonly IUserRepository _userRepository;
        private readonly INotificationRepository _repository;

        public NotificationService(IUserRepository userRepository, INotificationRepository repository)
        {
            _userRepository = userRepository;
            _repository = repository;
        }

        public Task<List<Notification>> GetNotificationsByUserIdAsync(Guid userId)
        {
            return _repository.GetNotificationsByUserIdAsync(userId);
        }

        public Task AddNotificationAsync(Notification notification)
        {
            return _repository.AddNotificationAsync(notification);
        }

        public Task MarkAsReadAsync(Guid notificationId)
        {
            return _repository.MarkAsReadAsync(notificationId);
        }

        public async Task MarkAllAsReadAsync(Guid userId)
        {
            var notifications = await _repository.GetNotificationsByUserIdAsync(userId);
            foreach (var notification in notifications)
            {
                if (!notification.IsRead)
                    await _repository.MarkAsReadAsync(notification.NotificationId);
            }
        }

        public async Task DeleteNotificationAsync(Guid notificationId)
        {
            // Pretpostavimo da postoji Delete metoda u repozitoriju
            // Ako nemaš, trebaš ju dodati u INotificationRepository i NotificationRepository
            // await _repository.DeleteAsync(notificationId);
        }

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
