namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Tests;

public class CompareWithMinimalApiWithXml : CompareWithMinimalApiBase
{
    public CompareWithMinimalApiWithXml() : base(OpenApiDocumentLocator.FromControllerApiWithXml, new[] { "application/json", "application/xml" })
    { }
}
