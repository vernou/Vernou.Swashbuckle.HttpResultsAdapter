using Microsoft.OpenApi.Models;

namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Tests;

public class IgnoreMinimalApiEndpoint
{
    [Theory]
    [InlineData("/minimalapi/ok_204")]
    [InlineData("/minimalapi/ok_204-async")]
    public async Task CheckMinimalApiEndpointIsNotModified(string path)
    {
        // Arrange

        var document = await OpenApiDocumentLocator.FromControllerApi;
        var responses = document.Paths[path].Operations[OperationType.Get].Responses;

        // Assert

        var response = responses.ShouldHaveSingleItem();
        response.Key.ShouldBe("204");
        response.Value.Description.ShouldBe("Custom Description");
        response.Value.Content.ShouldBeEmpty();
    }
}
