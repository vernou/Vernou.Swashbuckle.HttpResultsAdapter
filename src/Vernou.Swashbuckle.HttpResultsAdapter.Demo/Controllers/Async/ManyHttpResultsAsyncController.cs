namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Controllers.Async;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public class ManyHttpResultsAsyncController : Controller
{
    [HttpGet("ok_nocontent-async")]
    public async Task<Results<Ok, NoContent>> ReturnOkOrNoContent() => await Task.FromResult(TypedResults.Ok());

    [HttpGet("ok_nocontent_notfound-async")]
    public async Task<Results<Ok, NoContent, NotFound>> ReturnOkOrNoContentOrNotFound() => await Task.FromResult(TypedResults.Ok());
}
