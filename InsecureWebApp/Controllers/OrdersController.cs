using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsecureWebApp.Data;
using InsecureWebApp.Models;

namespace InsecureWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders(int? userId)
        {
            // SECURITY VULNERABILITY: No authentication required
            // SECURITY VULNERABILITY: No authorization checks - any user can view any orders
            
            try
            {
                if (userId.HasValue)
                {
                    // SECURITY VULNERABILITY: SQL Injection via userId parameter
                    var userOrders = await _context.Orders
                        .FromSqlRaw($"SELECT * FROM Orders WHERE UserId = {userId}")
                        .ToListAsync();
                    return Ok(userOrders);
                }

                // SECURITY VULNERABILITY: Returns all orders without permission checks
                var allOrders = await _context.Orders.ToListAsync();
                return Ok(allOrders);
            }
            catch (Exception ex)
            {
                // SECURITY VULNERABILITY: Exposing detailed error information
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            // SECURITY VULNERABILITY: No authentication required
            // SECURITY VULNERABILITY: IDOR (Insecure Direct Object Reference) vulnerability
            
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    return NotFound($"Order {id} not found");
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                // SECURITY VULNERABILITY: Exposing detailed error information
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            // SECURITY VULNERABILITY: No authentication required
            // SECURITY VULNERABILITY: No authorization checks
            // SECURITY VULNERABILITY: No input validation
            // SECURITY VULNERABILITY: No business logic validation (e.g., stock checks)
            
            try
            {
                // SECURITY VULNERABILITY: Direct object manipulation
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                // SECURITY VULNERABILITY: Exposing detailed error information
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            // SECURITY VULNERABILITY: No authentication required
            // SECURITY VULNERABILITY: No authorization checks (users can modify others' orders)
            // SECURITY VULNERABILITY: No input validation
            
            if (id != order.Id)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                _context.Entry(order).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                return Ok(order);
            }
            catch (Exception ex)
            {
                // SECURITY VULNERABILITY: Exposing detailed error information
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            // SECURITY VULNERABILITY: No authentication required
            // SECURITY VULNERABILITY: No authorization checks
            // SECURITY VULNERABILITY: No audit trail for deletions
            
            try
            {
                var order = await _context.Orders.FindAsync(id);
                if (order == null)
                {
                    return NotFound();
                }

                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();

                return Ok(new { Message = $"Order {id} deleted successfully" });
            }
            catch (Exception ex)
            {
                // SECURITY VULNERABILITY: Exposing detailed error information
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpGet("admin/all-orders")]
        public async Task<IActionResult> GetAllOrdersAdmin()
        {
            // SECURITY VULNERABILITY: No authentication required
            // SECURITY VULNERABILITY: No authorization checks for admin functionality
            // SECURITY VULNERABILITY: Admin endpoint exposed without proper protection
            
            try
            {
                var orders = await _context.Orders
                    .Include(o => _context.Users.Where(u => u.Id == o.UserId).First())
                    .ToListAsync();

                return Ok(orders);
            }
            catch (Exception ex)
            {
                // SECURITY VULNERABILITY: Exposing detailed error information
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpPost("process-payment")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
        {
            // SECURITY VULNERABILITY: No authentication required
            // SECURITY VULNERABILITY: No input validation for financial data
            // SECURITY VULNERABILITY: No PCI compliance considerations
            // SECURITY VULNERABILITY: No rate limiting for payment processing
            
            try
            {
                // SECURITY VULNERABILITY: Simulated payment processing with no security
                var order = await _context.Orders.FindAsync(request.OrderId);
                if (order == null)
                {
                    return NotFound("Order not found");
                }

                // SECURITY VULNERABILITY: No validation of payment amount vs order total
                // SECURITY VULNERABILITY: Credit card data handled insecurely
                
                order.Status = "Paid";
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Payment processed successfully",
                    OrderId = order.Id,
                    Amount = request.Amount,
                    CreditCard = request.CreditCardNumber, // VULNERABILITY: Returning credit card info
                    CVV = request.CVV // VULNERABILITY: Returning CVV
                });
            }
            catch (Exception ex)
            {
                // SECURITY VULNERABILITY: Exposing detailed error information
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }
    }

    public class PaymentRequest
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string CreditCardNumber { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;
        public string ExpiryDate { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;
    }
}