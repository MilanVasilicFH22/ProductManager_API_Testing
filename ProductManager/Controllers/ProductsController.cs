using Microsoft.AspNetCore.Mvc;
using ProductManager.Data;
using ProductManager.Models;
using Microsoft.EntityFrameworkCore;


namespace ProductManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tenantId = HttpContext.Items["Tenant-ID"]?.ToString();
            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest("Tenant-ID is missing.");
            }

            var products = await _context.Products
                                         .Where(p => p.TenantId == tenantId)
                                         .ToListAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            var tenantId = HttpContext.Items["Tenant-ID"]?.ToString();
            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest("Tenant-ID is missing.");
            }

            product.TenantId = tenantId; // Assign tenant ID from middleware
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created product {ProductName} for tenant {TenantId}", product.Name, product.TenantId);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
        }
    }


}