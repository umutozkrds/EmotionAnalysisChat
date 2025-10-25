using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Chatapi.Models;

namespace Chatapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly ChatContext _context;
    private readonly ILogger<UserController> _logger;

    public UserController(ChatContext context, ILogger<UserController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.Nickname))
            {
                return BadRequest(new { error = "Nickname is required" });
            }

            // Trim and validate nickname length
            request.Nickname = request.Nickname.Trim();
            if (request.Nickname.Length < 2)
            {
                return BadRequest(new { error = "Nickname must be at least 2 characters long" });
            }

            if (request.Nickname.Length > 50)
            {
                return BadRequest(new { error = "Nickname must not exceed 50 characters" });
            }

            // Check if nickname already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Nickname.ToLower() == request.Nickname.ToLower());

            if (existingUser != null)
            {
                return Conflict(new { error = "Nickname already exists. Please choose a different nickname." });
            }

            // Create new user
            var user = new User
            {
                Nickname = request.Nickname,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"New user registered: {user.Nickname} (ID: {user.Id})");

            return Ok(new
            {
                id = user.Id,
                nickname = user.Nickname,
                createdAt = user.CreatedAt,
                message = "User registered successfully"
            });
        }
        catch (DbUpdateException ex)
        {
            // Handle database constraint violations
            _logger.LogError(ex, "Database error during user registration");
            return Conflict(new { error = "Nickname already exists. Please choose a different nickname." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration");
            return StatusCode(500, new { error = "An error occurred while registering the user" });
        }
    }


    [HttpGet("login/{nickname}")]
    public async Task<IActionResult> Login(string nickname)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nickname))
            {
                return BadRequest(new { error = "Nickname is required" });
            }

            var user = await _context.Users
                .Where(u => u.Nickname.ToLower() == nickname.ToLower())
                .Select(u => new
                {
                    id = u.Id,
                    nickname = u.Nickname,
                    createdAt = u.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { error = "User not found. Please register first." });
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user login");
            return StatusCode(500, new { error = "An error occurred while retrieving user information" });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _context.Users
                .OrderByDescending(u => u.CreatedAt)
                .Select(u => new
                {
                    id = u.Id,
                    nickname = u.Nickname,
                    createdAt = u.CreatedAt
                })
                .ToListAsync();

            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return StatusCode(500, new { error = "An error occurred while retrieving users" });
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        try
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    id = u.Id,
                    nickname = u.Nickname,
                    createdAt = u.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user");
            return StatusCode(500, new { error = "An error occurred while retrieving user information" });
        }
    }


    [HttpGet("check-availability/{nickname}")]
    public async Task<IActionResult> CheckAvailability(string nickname)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(nickname))
            {
                return Ok(new { available = false, message = "Nickname cannot be empty" });
            }

            nickname = nickname.Trim();

            if (nickname.Length < 2)
            {
                return Ok(new { available = false, message = "Nickname must be at least 2 characters long" });
            }

            if (nickname.Length > 50)
            {
                return Ok(new { available = false, message = "Nickname must not exceed 50 characters" });
            }

            var exists = await _context.Users
                .AnyAsync(u => u.Nickname.ToLower() == nickname.ToLower());

            return Ok(new
            {
                available = !exists,
                nickname = nickname,
                message = exists ? "Nickname is already taken" : "Nickname is available"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking nickname availability");
            return StatusCode(500, new { error = "An error occurred while checking nickname availability" });
        }
    }
}

public class RegisterRequest
{
    public string Nickname { get; set; } = string.Empty;
}
