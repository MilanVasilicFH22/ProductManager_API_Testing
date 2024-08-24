using ProductManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace ProductManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        private readonly string _tenantId;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _tenantId = httpContextAccessor.HttpContext.Request.Headers["Tenant-ID"];
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasQueryFilter(p => p.TenantId == _tenantId);  // Filter data by TenantId
        }
    }
}
