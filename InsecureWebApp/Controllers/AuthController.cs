using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using InsecureWebApp.Data;
using InsecureWebApp.Models;

namespace InsecureWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // SECURITY VULNERABILITY: No input validation
            // SECURITY VULNERABILITY: No rate limiting on registration
            // SECURITY VULNERABILITY: No CAPTCHA or bot protection
            
            // SECURITY VULNERABILITY: Plain text password storage
            var user = new User
            {
                Email = request.Email,
                Password = request.Password, // VULNERABILITY: Storing plain text password
                FirstName = request.FirstName,
                LastName = request.LastName,
                SocialSecurityNumber = request.SocialSecurityNumber,
                CreditCardNumber = request.CreditCardNumber
            };

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // SECURITY VULNERABILITY: Exposing sensitive information in response
                return Ok(new
                {
                    Message = "User registered successfully",
                    UserId = user.Id,
                    Email = user.Email,
                    SSN = user.SocialSecurityNumber, // VULNERABILITY: Exposing SSN
                    CreditCard = user.CreditCardNumber // VULNERABILITY: Exposing credit card
                });
            }
            catch (Exception ex)
            {
                // SECURITY VULNERABILITY: Exposing detailed error information
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            // SECURITY VULNERABILITY: SQL Injection vulnerability
            var user = await _context.Users
                .FromSqlRaw($"SELECT * FROM Users WHERE Email = '{request.Email}' AND Password = '{request.Password}'")
                .FirstOrDefaultAsync();

            if (user == null)
            {
                // SECURITY VULNERABILITY: Information disclosure - different error messages
                return Unauthorized("Invalid email or password. User not found.");
            }

            // SECURITY VULNERABILITY: JWT without expiration and weak secret
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("weak-secret-key"); // VULNERABILITY: Weak key
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("isAdmin", user.IsAdmin.ToString()),
                    new Claim("ssn", user.SocialSecurityNumber), // VULNERABILITY: Sensitive data in token
                    new Claim("creditCard", user.CreditCardNumber) // VULNERABILITY: Sensitive data in token
                }),
                // VULNERABILITY: No expiration time set
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // SECURITY VULNERABILITY: No session management
            // SECURITY VULNERABILITY: No audit logging
            return Ok(new
            {
                Token = tokenString,
                User = new
                {
                    user.Id,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    user.IsAdmin,
                    user.SocialSecurityNumber, // VULNERABILITY: Exposing sensitive data
                    user.CreditCardNumber // VULNERABILITY: Exposing sensitive data
                }
            });
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            // SECURITY VULNERABILITY: No authentication required
            // SECURITY VULNERABILITY: No authorization checks
            // SECURITY VULNERABILITY: Exposing all user data including sensitive information
            
            var users = await _context.Users.ToListAsync();
            return Ok(users); // VULNERABILITY: Returns all sensitive data
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            // SECURITY VULNERABILITY: No authentication required
            // SECURITY VULNERABILITY: No authorization checks (IDOR vulnerability)
            
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user); // VULNERABILITY: Returns sensitive data
        }
    }

    public class RegisterRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string SocialSecurityNumber { get; set; } = string.Empty;
        public string CreditCardNumber { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}