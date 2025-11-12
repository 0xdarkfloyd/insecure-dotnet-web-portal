using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InsecureWebApp.Data;
using InsecureWebApp.Models;

namespace InsecureWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(string? search)
        {
            // SECURITY VULNERABILITY: SQL Injection via search parameter
            if (!string.IsNullOrEmpty(search))
            {
                var products = await _context.Products
                    .FromSqlRaw($"SELECT * FROM Products WHERE Name LIKE '%{search}%' OR Description LIKE '%{search}%'")
                    .ToListAsync();
                return Ok(products);
            }

            return Ok(await _context.Products.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            // SECURITY VULNERABILITY: No input validation
            // SECURITY VULNERABILITY: Potential information disclosure
            
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound($"Product with ID {id} not found in database");
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                // SECURITY VULNERABILITY: Exposing detailed error information
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            // SECURITY VULNERABILITY: No authentication required
            // SECURITY VULNERABILITY: No authorization checks
            // SECURITY VULNERABILITY: No input validation
            // SECURITY VULNERABILITY: No rate limiting
            
            try
            {
                // SECURITY VULNERABILITY: Direct object manipulation without validation
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                // SECURITY VULNERABILITY: Exposing detailed error information
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            // SECURITY VULNERABILITY: No authentication required
            // SECURITY VULNERABILITY: No authorization checks
            // SECURITY VULNERABILITY: No input validation
            // SECURITY VULNERABILITY: Mass assignment vulnerability
            
            if (id != product.Id)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                _context.Entry(product).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                
                return Ok(product);
            }
            catch (Exception ex)
            {
                // SECURITY VULNERABILITY: Exposing detailed error information
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            // SECURITY VULNERABILITY: No authentication required
            // SECURITY VULNERABILITY: No authorization checks
            // SECURITY VULNERABILITY: No confirmation required for destructive action
            
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound();
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return Ok(new { Message = $"Product {id} deleted successfully" });
            }
            catch (Exception ex)
            {
                // SECURITY VULNERABILITY: Exposing detailed error information
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpPost("bulk-update")]
        public async Task<IActionResult> BulkUpdateProducts([FromBody] List<Product> products)
        {
            // SECURITY VULNERABILITY: No authentication required
            // SECURITY VULNERABILITY: No authorization checks
            // SECURITY VULNERABILITY: No rate limiting for bulk operations
            // SECURITY VULNERABILITY: No input validation
            
            try
            {
                foreach (var product in products)
                {
                    _context.Entry(product).State = EntityState.Modified;
                }
                
                await _context.SaveChangesAsync();
                return Ok(new { Message = $"Updated {products.Count} products" });
            }
            catch (Exception ex)
            {
                // SECURITY VULNERABILITY: Exposing detailed error information
                return BadRequest(new { Error = ex.Message, StackTrace = ex.StackTrace });
            }
        }
    }
}