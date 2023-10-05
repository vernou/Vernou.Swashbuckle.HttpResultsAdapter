namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Controllers.Async;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using Vernou.Swashbuckle.HttpResultsAdapter.Demo.Models;

public class HttpResultsOfAsyncController : Controller
{
    [HttpGet("ok-foo-async")]
    public async Task<Ok<Foo>> ReturnOkOfAsync() => await Task.FromResult(TypedResults.Ok(new Foo()));

    [HttpGet("created-foo-async")]
    public async Task<Created<Foo>> ReturnCreatedOf() => await Task.FromResult(TypedResults.Created("", new Foo()));

    [HttpGet("createdatroute-foo-async")]
    public async Task<CreatedAtRoute<Foo>> ReturnCreatedAtRouteOfAsync() => await Task.FromResult(TypedResults.CreatedAtRoute(new Foo()));

    [HttpGet("accepted-foo-async")]
    public async Task<Accepted<Foo>> ReturnAcceptedOfAsync() => await Task.FromResult(TypedResults.Accepted("", new Foo()));

    [HttpGet("acceptedatroute-foo-async")]
    public async Task<AcceptedAtRoute<Foo>> ReturnAcceptedAtRouteOfAsync() => await Task.FromResult(TypedResults.AcceptedAtRoute(new Foo()));

    [HttpGet("badrequest-foo-async")]
    public async Task<BadRequest<Foo>> ReturnBadRequestOfAsync() => await Task.FromResult(TypedResults.BadRequest(new Foo()));

    [HttpGet("conflict-foo-async")]
    public async Task<Conflict<Foo>> ReturnConflictOfAsync() => await Task.FromResult(TypedResults.Conflict(new Foo()));

    [HttpGet("notfound-foo-async")]
    public async Task<NotFound<Foo>> ReturnNotFoundOfAsync() => await Task.FromResult(TypedResults.NotFound(new Foo()));

    [HttpGet("unprocessableentity-foo-async")]
    public async Task<UnprocessableEntity<Foo>> ReturnUnprocessableEntityOfAsync() => await Task.FromResult(TypedResults.UnprocessableEntity(new Foo()));
}
