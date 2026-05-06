using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderManagement.API.Data;
using OrderManagement.API.DTOs;
using OrderManagement.API.Models;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<User> _hasher;

        public AuthController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _hasher = new PasswordHasher<User>();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var existingUser = _context.Users
                .FirstOrDefault(u => u.Username == dto.Username);

            if (existingUser != null)
                return BadRequest("Username already exists");

            var user = new User
            {
                Username = dto.Username
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult Login(LoginDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("Username and password are required.");
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == dto.Username);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            var verificationResult = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (verificationResult != PasswordVerificationResult.Success)
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = GenerateJwtToken(user);
            return Ok(new { token });
        }

        private string GenerateJwtToken(User user)
        {
            var jwtKey = _config["Jwt:Key"];
            var issuer = _config["Jwt:Issuer"];
            var audience = _config["Jwt:Audience"];
            var expiresInMinutes = _config.GetValue<int?>("Jwt:ExpiresInMinutes") ?? 60;

            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
            {
                throw new InvalidOperationException("JWT configuration is missing. Please add Jwt settings to appsettings.json.");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult GetUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(new { user.Id, user.Username, user.Role, user.CreatedAt });
        }

        [HttpGet("admin/users")]
        [Authorize(Roles = "Admin")]
        public ActionResult<List<object>> GetAllUsers()
        {
            var users = _context.Users
                .Select(u => new { u.Id, u.Username, u.Role, u.CreatedAt })
                .ToList();
            return Ok(users);
        }

        [HttpPut("admin/users/{id}/role")]
        [Authorize(Roles = "Admin")]
        public ActionResult SetUserRole(int id, [FromBody] SetRoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Role) || (dto.Role != "Admin" && dto.Role != "User"))
            {
                return BadRequest("Role must be 'Admin' or 'User'.");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            user.Role = dto.Role;
            _context.SaveChanges();

            return Ok(new { message = $"User role updated to {dto.Role}", user.Id, user.Username, user.Role });
        }
    }
}