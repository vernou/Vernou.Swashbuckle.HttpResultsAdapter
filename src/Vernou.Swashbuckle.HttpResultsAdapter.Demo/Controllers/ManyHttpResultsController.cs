namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Controllers;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public class ManyHttpResultsController
{
    [HttpGet("ok_nocontent")]
    public Results<Ok, NoContent> ReturnOkOrNoContent() => TypedResults.Ok();

    [HttpGet("ok_nocontent-async")]
    public async Task<Results<Ok, NoContent>> ReturnOkOrNoContentAsync() => await Task.FromResult(TypedResults.Ok());

    [HttpGet("ok_nocontent_notfound")]
    public Results<Ok, NoContent, NotFound> ReturnOkOrNoContentOrNotFound() => TypedResults.Ok();

    [HttpGet("ok_nocontent_notfound-async")]
    public async Task<Results<Ok, NoContent, NotFound>> ReturnOkOrNoContentOrNotFoundAsync() => await Task.FromResult(TypedResults.Ok());
}
