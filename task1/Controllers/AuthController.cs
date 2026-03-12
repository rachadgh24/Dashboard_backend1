using Microsoft.AspNetCore.Mvc;
using task1.DataLayer.Entities;
using task1.DataLayer.Interfaces;

namespace task1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;
        private readonly IUserRepository _userRepository;

        public AuthController(
            JwtService jwtService,
            IUserRepository userRepository)
        {
            _jwtService = jwtService;
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var email = request.Email.Trim();
            var user = await _userRepository.GetByEmailAsync(email);
            if (user != null && BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                var token = _jwtService.GenerateToken(user.Id.ToString(), user.Email, user.Name, user.Role);
                return Ok(new { token });
            }

            return Unauthorized("Invalid email or password.");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var normalizedEmail = request.Email.Trim();
            var existingUser = await _userRepository.GetByEmailAsync(normalizedEmail);
            if (existingUser != null)
            {
                return Conflict("Email is already registered.");
            }

            var newUser = new User
            {
                Email = normalizedEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Name = request.Name?.Trim() ?? string.Empty,
                Role = "Admin"
            };

            var createdUser = await _userRepository.AddUserAsync(newUser);
            await _userRepository.SaveChangesAsync();

            var token = _jwtService.GenerateToken(createdUser.Id.ToString(), createdUser.Email, createdUser.Name, createdUser.Role);
            return Ok(new { token });
        }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? Name { get; set; }
    }
}