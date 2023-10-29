using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;

namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Tests;

internal static class OpenApiDocumentLocator
{
    public static Task<OpenApiDocument> FromControllerApi { get; } = Get<WebApplicationFactory<Program>, Program>();
    public static Task<OpenApiDocument> FromControllerApiWithXml { get; } = Get<ApiWithXmlFactory, Program>();
    public static Task<OpenApiDocument> FromMinimalApi { get; } = Get<WebApplicationFactory<MinimalApi.Program>, MinimalApi.Program>();

    private static async Task<OpenApiDocument> Get<TFactory, TProgram>()
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

    private class ApiWithXmlFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);
            builder.ConfigureTestServices(services =>
                services.AddControllers().AddXmlSerializerFormatters()
            );
        }
    }
}
