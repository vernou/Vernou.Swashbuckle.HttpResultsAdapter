using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Tests;

public class CompareWithMinimalApiWithXml : CompareWithMinimalApiBase
{

    public CompareWithMinimalApiWithXml() : base(ControllerOpenApiDocument, new[] { "application/json", "application/xml" })
    { }

    private readonly static Task<OpenApiDocument> ControllerOpenApiDocument = OpenApiDocumentGetter.Get<ApiWithXmlFactory, Program>();

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
