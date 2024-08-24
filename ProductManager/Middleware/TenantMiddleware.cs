namespace ProductManager.Middleware
{
    public class TenantMiddleware
    {
        private readonly RequestDelegate _next;

        public TenantMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check for Tenant-ID in the request headers
            if (!context.Request.Headers.TryGetValue("Tenant-ID", out var tenantId))
            {
                context.Response.StatusCode = 400; // Bad Request
                await context.Response.WriteAsync("Tenant-ID is missing.");
                return;
            }

            // Store Tenant-ID in HttpContext.Items for later use in the controller
            context.Items["Tenant-ID"] = tenantId.ToString();

            // Continue to the next middleware
            await _next(context);
        }
    }

}
