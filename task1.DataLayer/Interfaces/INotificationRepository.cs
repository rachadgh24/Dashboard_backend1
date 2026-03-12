using task1.DataLayer.Entities;

namespace task1.DataLayer.Interfaces
{
    public interface INotificationRepository
    {
        Task<Notification> AddAsync(Notification notification);
        Task<List<Notification>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
        Task<int> DeleteAllAsync();
        Task SaveChangesAsync();
    }
}
