using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Tests;

public class CompareWithMinimalApi
{
    public static IEnumerable<object[]> CheckResponsesData()
    {
        using var factory = new WebApplicationFactory<MinimalApi.Program>();
        var paths = factory
            .Services
            .GetRequiredService<EndpointDataSource>()
            .Endpoints
            .Cast<RouteEndpoint>()
            .Select(e => new object[] { "/" + (e.RoutePattern.RawText ?? "") })
            .ToArray();
        return paths;
    }

    [Theory]
    [MemberData(nameof(CheckResponsesData))]
    public async Task CheckResponses(string path)
    {
        // Arrange

        var sutDoc = await ControllerOpenApiDocument;
        var expDoc = await MinimalApiOpenApiDocument;

        // Assert

        var sutPath = sutDoc.Paths[path];
        var expPath = expDoc.Paths[path];

        var sutResponses = sutPath.Operations[OperationType.Get].Responses;
        var expResponses = expPath.Operations[OperationType.Get].Responses;

        sutResponses.Count.ShouldBe(expResponses.Count);
        foreach(var responseKey in expResponses.Keys)
        {
            var sutResponse = sutResponses[responseKey];
            var expResponse = expResponses[responseKey];

            sutResponse.Description.ShouldBe(expResponse.Description);

            sutResponse.Content.Count.ShouldBe(expResponse.Content.Count);
            foreach(var contentKey in expResponse.Content.Keys)
            {
                var sutContent = sutResponse.Content[contentKey];
                var expContent = expResponse.Content[contentKey];

                sutContent.Schema.Type.ShouldBe(expContent.Schema.Type);
                sutContent.Schema.Reference.Id.ShouldBe(expContent.Schema.Reference.Id);
            }
        }
    }

    private readonly static Task<OpenApiDocument> ControllerOpenApiDocument = GetOpenApi<Program>();
    private readonly static Task<OpenApiDocument> MinimalApiOpenApiDocument = GetOpenApi<MinimalApi.Program>();

    private static async Task<OpenApiDocument> GetOpenApi<T>() where T : class
    {
        using var factory = new WebApplicationFactory<T>();
        using var client = factory.CreateClient();
        using var httpResponse = await client.GetAsync("/swagger/v1/swagger.json");
        httpResponse.EnsureSuccessStatusCode();
        using var stream = httpResponse.Content.ReadAsStream();
        var document = new OpenApiStreamReader().Read(stream, out var diagnostic);
        diagnostic.Errors.ShouldBeEmpty();
        diagnostic.Warnings.ShouldBeEmpty();
        return document;
    }
}
