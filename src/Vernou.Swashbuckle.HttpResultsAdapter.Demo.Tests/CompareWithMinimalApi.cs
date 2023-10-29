namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Tests;

public class CompareWithMinimalApi : CompareWithMinimalApiBase
{
    public CompareWithMinimalApi() : base(OpenApiDocumentLocator.FromControllerApi, new[] { "application/json" })
    { }
}
