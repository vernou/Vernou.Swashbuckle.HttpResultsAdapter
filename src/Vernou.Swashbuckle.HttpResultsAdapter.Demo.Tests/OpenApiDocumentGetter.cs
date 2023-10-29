using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Tests;

internal static class OpenApiDocumentGetter
{
    public static async Task<OpenApiDocument> Get<TFactory, TProgram>()
    where TFactory : WebApplicationFactory<TProgram>, new()
    where TProgram : class
    {
        using var factory = new TFactory();
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