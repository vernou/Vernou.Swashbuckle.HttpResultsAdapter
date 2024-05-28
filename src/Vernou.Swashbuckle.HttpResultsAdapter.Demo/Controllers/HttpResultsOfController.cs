namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Controllers;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Vernou.Swashbuckle.HttpResultsAdapter.Demo.Models;

public class HttpResultsOfController
{
    [HttpGet("ok-foo")]
    public Ok<Foo> ReturnOkOf() => TypedResults.Ok(new Foo());

    [HttpGet("ok-foo-async")]
    public async Task<Ok<Foo>> ReturnOkOfAsync() => await Task.FromResult(TypedResults.Ok(new Foo()));

    [HttpGet("ok-foo-valuetask")]
    public async ValueTask<Ok<Foo>> ReturnOkOfValueTask() => await Task.FromResult(TypedResults.Ok(new Foo()));

    [HttpGet("created-foo")]
    public Created<Foo> ReturnCreatedOf() => TypedResults.Created("", new Foo());

    [HttpGet("created-foo-async")]
    public async Task<Created<Foo>> ReturnCreatedOfAsync() => await Task.FromResult(TypedResults.Created("", new Foo()));

    [HttpGet("created-foo-valuetask")]
    public async ValueTask<Created<Foo>> ReturnCreatedOfValueTask() => await Task.FromResult(TypedResults.Created("", new Foo()));

    [HttpGet("createdatroute-foo")]
    public CreatedAtRoute<Foo> ReturnCreatedAtRouteOf() => TypedResults.CreatedAtRoute(new Foo());

    [HttpGet("createdatroute-foo-async")]
    public async Task<CreatedAtRoute<Foo>> ReturnCreatedAtRouteOfAsync() => await Task.FromResult(TypedResults.CreatedAtRoute(new Foo()));

    [HttpGet("createdatroute-foo-valuetask")]
    public async ValueTask<CreatedAtRoute<Foo>> ReturnCreatedAtRouteOfValueTask() => await Task.FromResult(TypedResults.CreatedAtRoute(new Foo()));

    [HttpGet("accepted-foo")]
    public Accepted<Foo> ReturnAcceptedOf() => TypedResults.Accepted("", new Foo());

    [HttpGet("accepted-foo-async")]
    public async Task<Accepted<Foo>> ReturnAcceptedOfAsync() => await Task.FromResult(TypedResults.Accepted("", new Foo()));

    [HttpGet("accepted-foo-valuetask")]
    public async ValueTask<Accepted<Foo>> ReturnAcceptedOfValueTask() => await Task.FromResult(TypedResults.Accepted("", new Foo()));

    [HttpGet("acceptedatroute-foo")]
    public AcceptedAtRoute<Foo> ReturnAcceptedAtRouteOf() => TypedResults.AcceptedAtRoute(new Foo());

    [HttpGet("acceptedatroute-foo-async")]
    public async Task<AcceptedAtRoute<Foo>> ReturnAcceptedAtRouteOfAsync() => await Task.FromResult(TypedResults.AcceptedAtRoute(new Foo()));

    [HttpGet("acceptedatroute-foo-valuetask")]
    public async ValueTask<AcceptedAtRoute<Foo>> ReturnAcceptedAtRouteOfValueTask() => await Task.FromResult(TypedResults.AcceptedAtRoute(new Foo()));

    [HttpGet("badrequest-foo")]
    public BadRequest<Foo> ReturnBadRequestOf() => TypedResults.BadRequest(new Foo());

    [HttpGet("badrequest-foo-async")]
    public async Task<BadRequest<Foo>> ReturnBadRequestOfAsync() => await Task.FromResult(TypedResults.BadRequest(new Foo()));

    [HttpGet("badrequest-foo-valuetask")]
    public async ValueTask<BadRequest<Foo>> ReturnBadRequestOfValueTask() => await Task.FromResult(TypedResults.BadRequest(new Foo()));

    [HttpGet("conflict-foo")]
    public Conflict<Foo> ReturnConflictOf() => TypedResults.Conflict(new Foo());

    [HttpGet("conflict-foo-async")]
    public async Task<Conflict<Foo>> ReturnConflictOfAsync() => await Task.FromResult(TypedResults.Conflict(new Foo()));

    [HttpGet("conflict-foo-valuetask")]
    public async ValueTask<Conflict<Foo>> ReturnConflictOfValueTask() => await Task.FromResult(TypedResults.Conflict(new Foo()));

    [HttpGet("notfound-foo")]
    public NotFound<Foo> ReturnNotFoundOf() => TypedResults.NotFound(new Foo());

    [HttpGet("notfound-foo-async")]
    public async Task<NotFound<Foo>> ReturnNotFoundOfAsync() => await Task.FromResult(TypedResults.NotFound(new Foo()));

    [HttpGet("notfound-foo-valuetask")]
    public async ValueTask<NotFound<Foo>> ReturnNotFoundOfValueTask() => await Task.FromResult(TypedResults.NotFound(new Foo()));

    [HttpGet("unprocessableentity-foo")]
    public UnprocessableEntity<Foo> ReturnUnprocessableEntityOf() => TypedResults.UnprocessableEntity(new Foo());

    [HttpGet("unprocessableentity-foo-async")]
    public async Task<UnprocessableEntity<Foo>> ReturnUnprocessableEntityOfAsync() => await Task.FromResult(TypedResults.UnprocessableEntity(new Foo()));

    [HttpGet("unprocessableentity-foo-valuetask")]
    public async ValueTask<UnprocessableEntity<Foo>> ReturnUnprocessableEntityOfValueTask() => await Task.FromResult(TypedResults.UnprocessableEntity(new Foo()));
}
