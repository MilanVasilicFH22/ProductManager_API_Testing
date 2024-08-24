using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductManager.Data;
using ProductManager.Filter;
using ProductManager.Middleware;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("MultiTenantDb"));
builder.Services.AddHttpContextAccessor();

// Add Swagger configuration
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.OperationFilter<TenantHeaderOperationFilter>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger";  // Set Swagger UI at /swagger path
    });
}

// Optionally redirect to /swagger when accessing the root URL
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }

    await next();
});

app.UseMiddleware<TenantMiddleware>();

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
