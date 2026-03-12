using Microsoft.EntityFrameworkCore;
using task1.DataLayer.DbContexts;
using task1.DataLayer.Entities;
using task1.DataLayer.Interfaces;

namespace task1.DataLayer.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DotNetTrainingCoreContext _context;

        public NotificationRepository(DotNetTrainingCoreContext context)
        {
            _context = context;
        }

        public Task<Notification> AddAsync(Notification notification)
        {
            var entity = _context.Notifications.Add(notification);
            return Task.FromResult(entity.Entity);
        }

        public async Task<List<Notification>> GetAllAsync()
        {
            return await _context.Notifications
                .AsNoTracking()
                .OrderByDescending(n => n.CreatedAt)
                .ThenByDescending(n => n.Id)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Notifications.FindAsync(id);
            if (entity == null) return false;

            _context.Notifications.Remove(entity);
            return true;
        }

        public async Task<int> DeleteAllAsync()
        {
            var all = await _context.Notifications.ToListAsync();
            if (all.Count == 0) return 0;

            _context.Notifications.RemoveRange(all);
            return all.Count;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
