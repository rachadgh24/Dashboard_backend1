using task1.Application.Models;

namespace task1.Application.Interfaces
{
    public interface INotificationService
    {
        Task RecordAsync(string message);
        Task<List<NotificationModel>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
        Task<int> ClearAsync();
    }
}
