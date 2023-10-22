using Vernou.Swashbuckle.HttpResultsAdapter;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.OperationFilter<HttpResultsOperationFilter>());

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/ok-foo", () => TypedResults.Ok(new Foo(0, "")))
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

internal record Foo(int Id, string Label);