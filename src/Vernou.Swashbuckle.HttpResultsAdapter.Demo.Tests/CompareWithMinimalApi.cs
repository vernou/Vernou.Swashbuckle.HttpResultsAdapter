using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.OpenApi.Models;

namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Tests;

public class CompareWithMinimalApi : CompareWithMinimalApiBase
{

    public CompareWithMinimalApi() : base(ControllerOpenApiDocument, new[] { "application/json" })
    { }

    private readonly static Task<OpenApiDocument> ControllerOpenApiDocument = OpenApiDocumentGetter.Get<WebApplicationFactory<Program>, Program>();
}
