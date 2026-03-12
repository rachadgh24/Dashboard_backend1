using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using task1.Application.Interfaces;
using task1.Application.Models;

namespace task1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET /notifications
        [HttpGet]
        public async Task<ActionResult<List<NotificationModel>>> GetNotifications()
        {
            var notifications = await _notificationService.GetAllAsync();
            return Ok(notifications);
        }

        // DELETE /notifications/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            var deleted = await _notificationService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        // DELETE /notifications (clear all)
        [HttpDelete]
        public async Task<IActionResult> ClearNotifications()
        {
            await _notificationService.ClearAsync();
            return NoContent();
        }
    }
}
