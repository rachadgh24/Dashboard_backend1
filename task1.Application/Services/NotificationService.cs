using task1.Application.Interfaces;
using task1.Application.Models;
using task1.DataLayer.Entities;
using task1.DataLayer.Interfaces;

namespace task1.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task RecordAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            var notification = new Notification
            {
                Message = message.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await _notificationRepository.AddAsync(notification);
            await _notificationRepository.SaveChangesAsync();
        }

        public async Task<List<NotificationModel>> GetAllAsync()
        {
            var notifications = await _notificationRepository.GetAllAsync();
            return notifications
                .Select(n => new NotificationModel
                {
                    Id = n.Id,
                    Message = n.Message,
                    CreatedAt = n.CreatedAt
                })
                .ToList();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var deleted = await _notificationRepository.DeleteAsync(id);
            if (!deleted) return false;

            await _notificationRepository.SaveChangesAsync();
            return true;
        }

        public async Task<int> ClearAsync()
        {
            var count = await _notificationRepository.DeleteAllAsync();
            if (count > 0)
            {
                await _notificationRepository.SaveChangesAsync();
            }

            return count;
        }
    }
}
