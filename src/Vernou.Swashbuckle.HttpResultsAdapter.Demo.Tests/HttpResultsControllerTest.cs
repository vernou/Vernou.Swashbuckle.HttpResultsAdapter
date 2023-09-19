namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Tests;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

public class HttpResultsControllerTest
{
    [Fact]
    public async Task OkAs200()
    {
        // Arrange

        var factory = new WebApplicationFactory<Program>();
        var client = factory.CreateClient();
        var httpResponse = await client.GetAsync("/swagger/v1/swagger.json");
        httpResponse.EnsureSuccessStatusCode();
        var document = new OpenApiStreamReader().Read(await httpResponse.Content.ReadAsStreamAsync(), out var diagnostic);

        // Act

        var responses = document.Paths["/ok"].Operations[OperationType.Get].Responses;

        //Assert

        diagnostic.Errors.ShouldBeEmpty();
        diagnostic.Warnings.ShouldBeEmpty();

        var response = responses.ShouldHaveSingleItem();
        response.Key.ShouldBe("200");
        response.Value.Content.ShouldBeEmpty();
    }
}
