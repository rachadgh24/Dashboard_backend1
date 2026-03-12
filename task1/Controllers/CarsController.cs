using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using task1.Application.Interfaces;
using task1.Application.Models;
using task1;
using task1.Application.Services;

namespace task1.Controllers{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;
        private readonly INotificationService _notificationService;

        public CarsController(ICarService carService, INotificationService notificationService)
        {
            _carService = carService;
            _notificationService = notificationService;
        }
    
    [Authorize(Roles = "Admin,General Manager")]
    [HttpGet]
    public async Task<List<CarModel>> GetCars()
    {
        return await _carService.GetAllAsync();
    }
    [Authorize(Roles = "Admin,General Manager")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCar(int id)
    {
        var car = await _carService.GetByIdAsync(id);
        if (car == null) return NotFound();
        return Ok(car);
    }
    [Authorize(Roles = "Admin,General Manager")]
    [HttpPost]
    public async Task<IActionResult> AddCar([FromBody] CarModel? carModel)
    {
        if (carModel == null) return BadRequest();
        var addedCar = await _carService.AddCarAsync(carModel);
        var role = User.FindFirst("role")?.Value ?? string.Empty;
        if (role == UserRoles.GeneralManager || role == UserRoles.SocialMediaManager)
        {
            var name = ActionNotificationHelper.GetDisplayName(User);
            var message = ActionNotificationHelper.Format(role, name, "added a car");
            await _notificationService.RecordAsync(message);
            return Ok(new { car = addedCar, message });
        }
        return Ok(addedCar);
    }
    [Authorize(Roles = "Admin,General Manager")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCar(int id, [FromBody] CarModel? carModel)
    {
        if (carModel == null) return BadRequest();
        var updatedCar = await _carService.UpdateCarAsync(id, carModel);
        if (updatedCar == null) return NotFound();
        return Ok(updatedCar);
    }
    [Authorize(Roles = "Admin,General Manager")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCar(int id)
    {
        if (!await _carService.DeleteCarAsync(id)) return NotFound();
        return Ok();
    }

    [Authorize(Roles = "Admin,General Manager")]
    [HttpGet("paginate")]
    public async Task<IActionResult> PaginateCars([FromQuery] int page = 1)
    {
        var cars = await _carService.PaginateCarsAsync(page);
        return Ok(cars);
    }

    [Authorize(Roles = "Admin,General Manager")]
    [HttpGet("count")]
    public async Task<IActionResult> GetCarsCount()
    {
        var count = await _carService.GetCountAsync();
        return Ok(count);
    }
}
}