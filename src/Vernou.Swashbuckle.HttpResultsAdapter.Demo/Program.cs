using Microsoft.OpenApi.Models;
using Vernou.Swashbuckle.HttpResultsAdapter;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<HttpResultsOperationFilter>();
    options.EnableAnnotations();
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

// Generate custom OpenApiResponses to check the adapter don't modify reponse from minimal API
var configureOperation = (OpenApiOperation o) =>
    new OpenApiOperation {
        Responses = new OpenApiResponses {
            { "204", new OpenApiResponse { Description = "Custom Description" } }
        }
    };
app.MapGet("minimalapi/ok_204", () => TypedResults.Ok()).WithOpenApi(configureOperation);
app.MapGet("minimalapi/ok_204-async", () => Task.FromResult(TypedResults.Ok())).WithOpenApi(configureOperation);

app.Run();

public partial class Program { }
