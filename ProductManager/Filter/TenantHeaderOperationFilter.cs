using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ProductManager.Filter
{
    public class TenantHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Tenant-ID",
                In = ParameterLocation.Header,
                Description = "Tenant ID",
                Required = true, // Set to true if this header is mandatory for the endpoint
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
        }
    }
}
