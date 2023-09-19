namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Controllers;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public class HttpResultsController
{
    [HttpGet("ok")]
    public Ok ReturnOk() => TypedResults.Ok();
}
