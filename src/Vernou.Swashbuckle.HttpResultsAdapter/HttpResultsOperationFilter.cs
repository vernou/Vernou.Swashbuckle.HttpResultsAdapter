namespace Vernou.Swashbuckle.HttpResultsAdapter;

using global::Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

/// <summary>
/// OperationFilter to generate OAS response to action that return HttpResults type
/// </summary>
public class HttpResultsOperationFilter : IOperationFilter
{
    void IOperationFilter.Apply(OpenApiOperation operation, OperationFilterContext context)
    {

    }
}
