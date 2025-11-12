using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using InsecureWebApp.Data;

namespace InsecureWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("system-info")]
        public IActionResult GetSystemInfo()
        {
            // SECURITY VULNERABILITY: No authentication required
            // SECURITY VULNERABILITY: Exposing sensitive system information
            
            try
            {
                var systemInfo = new
                {
                    ServerName = Environment.MachineName,
                    OSVersion = Environment.OSVersion.ToString(),
                    ProcessorCount = Environment.ProcessorCount,
                    WorkingSet = Environment.WorkingSet,
                    UserName = Environment.UserName, // VULNERABILITY: Exposing system user
                    CurrentDirectory = Environment.CurrentDirectory,
                    CommandLine = Environment.CommandLine,
                    EnvironmentVariables = Environment.GetEnvironmentVariables(), // VULNERABILITY: Exposing env vars
                    SystemDirectory = Environment.SystemDirectory,
                    Version = Environment.Version.ToString(),
                    DatabaseConnectionString = "Server=localhost;Database=InsecureApp;User Id=sa;Password=P@ssw0rd123" // VULNERABILITY: Exposing DB credentials
                };

                return Ok(systemInfo);
            }
            catch (Exception ex)
            {
                // SECURITY VULNERABILITY: Exposing detailed error information
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpPost("execute-command")]
        public IActionResult ExecuteCommand([FromBody] CommandRequest request)
        {
            // SECURITY VULNERABILITY: No authentication required
            // SECURITY VULNERABILITY: Command injection vulnerability - CRITICAL!
            // SECURITY VULNERABILITY: No input validation
            
            try
            {
                var processStartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c {request.Command}", // VULNERABILITY: Direct command execution
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using var process = System.Diagnostics.Process.Start(processStartInfo);
                var output = process?.StandardOutput.ReadToEnd();
                var error = process?.StandardError.ReadToEnd();
                process?.WaitForExit();

                return Ok(new
                {
                    Command = request.Command,
                    Output = output,
                    Error = error,
                    ExitCode = process?.ExitCode
                });
            }
            catch (Exception ex)
            {
                // SECURITY VULNERABILITY: Exposing detailed error information
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpGet("database-backup")]
        public async Task<IActionResult> BackupDatabase()
        {
            // SECURITY VULNERABILITY: No authentication required
            // SECURITY VULNERABILITY: No authorization checks
            // SECURITY VULNERABILITY: Exposing entire database
            
            try
            {
                var users = await _context.Users.ToListAsync();
                var products = await _context.Products.ToListAsync();
                var orders = await _context.Orders.ToListAsync();

                var backup = new
                {
                    Users = users, // VULNERABILITY: Including all sensitive user data
                    Products = products,
                    Orders = orders,
                    BackupDate = DateTime.Now,
                    BackupBy = "Anonymous" // VULNERABILITY: No user tracking
                };

                return Ok(backup);
            }
            catch (Exception ex)
            {
                // SECURITY VULNERABILITY: Exposing detailed error information
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpPost("restore-database")]
        public async Task<IActionResult> RestoreDatabase([FromBody] RestoreRequest request)
        {
            // SECURITY VULNERABILITY: No authentication required
            // SECURITY VULNERABILITY: No authorization checks
            // SECURITY VULNERABILITY: No validation of restore data
            // SECURITY VULNERABILITY: Potential data corruption
            
            try
            {
                // SECURITY VULNERABILITY: Truncating tables without proper validation
                _context.Users.RemoveRange(_context.Users);
                _context.Products.RemoveRange(_context.Products);
                _context.Orders.RemoveRange(_context.Orders);

                if (request.Users != null)
                {
                    _context.Users.AddRange(request.Users);
                }
                if (request.Products != null)
                {
                    _context.Products.AddRange(request.Products);
                }
                if (request.Orders != null)
                {
                    _context.Orders.AddRange(request.Orders);
                }

                await _context.SaveChangesAsync();

                return Ok(new { Message = "Database restored successfully" });
            }
            catch (Exception ex)
            {
                // SECURITY VULNERABILITY: Exposing detailed error information
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpDelete("delete-all-data")]
        public async Task<IActionResult> DeleteAllData([FromQuery] string confirm)
        {
            // SECURITY VULNERABILITY: No authentication required
            // SECURITY VULNERABILITY: Weak confirmation mechanism
            // SECURITY VULNERABILITY: No audit trail for destructive actions
            
            if (confirm != "YES-DELETE-EVERYTHING")
            {
                return BadRequest("Invalid confirmation");
            }

            try
            {
                _context.Orders.RemoveRange(_context.Orders);
                _context.Products.RemoveRange(_context.Products);
                _context.Users.RemoveRange(_context.Users);

                await _context.SaveChangesAsync();

                return Ok(new { Message = "All data deleted successfully" });
            }
            catch (Exception ex)
            {
                // SECURITY VULNERABILITY: Exposing detailed error information
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }
    }

    public class CommandRequest
    {
        public string Command { get; set; } = string.Empty;
    }

    public class RestoreRequest
    {
        public List<InsecureWebApp.Models.User>? Users { get; set; }
        public List<InsecureWebApp.Models.Product>? Products { get; set; }
        public List<InsecureWebApp.Models.Order>? Orders { get; set; }
    }
}