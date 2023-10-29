using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Tests;

public abstract class CompareWithMinimalApiBase
{
    private readonly Task<OpenApiDocument> _sut;
    private readonly string[] _contentTypes;
    private readonly Task<OpenApiDocument> _exp;

    public CompareWithMinimalApiBase(Task<OpenApiDocument> sut, string[] contentTypes)
    {
        _sut = sut;
        _contentTypes = contentTypes;
        _exp = OpenApiDocumentLocator.FromMinimalApi;
    }

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

        var sutDoc = await _sut;
        var expDoc = await _exp;

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

            if(expResponse.Content.Any())
            {
                sutResponse.Content.Count.ShouldBe(_contentTypes.Length);
                foreach(var contentType in _contentTypes)
                {
                    var sutContent = sutResponse.Content[contentType];
                    var expContent = expResponse.Content["application/json"];

                    sutContent.Schema.Type.ShouldBe(expContent.Schema.Type);
                    sutContent.Schema.Reference.Id.ShouldBe(expContent.Schema.Reference.Id);
                }
            }
            else
            {
                sutResponse.Content.ShouldBeEmpty();
            }
        }
    }
}
