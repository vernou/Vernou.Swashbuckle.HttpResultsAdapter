namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Controllers.Synchronous;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Vernou.Swashbuckle.HttpResultsAdapter.Demo.Models;

public class HttpResultsOfController : Controller
{
    [HttpGet("ok-foo")]
    public Ok<Foo> ReturnOkOf() => TypedResults.Ok(new Foo());

    [HttpGet("created-foo")]
    public Created<Foo> ReturnCreatedOf() => TypedResults.Created("", new Foo());

    [HttpGet("createdatroute-foo")]
    public CreatedAtRoute<Foo> ReturnCreatedAtRouteOf() => TypedResults.CreatedAtRoute(new Foo());

    [HttpGet("accepted-foo")]
    public Accepted<Foo> ReturnAcceptedOf() => TypedResults.Accepted("", new Foo());

    [HttpGet("acceptedatroute-foo")]
    public AcceptedAtRoute<Foo> ReturnAcceptedAtRouteOf() => TypedResults.AcceptedAtRoute(new Foo());

    [HttpGet("badrequest-foo")]
    public BadRequest<Foo> ReturnBadRequestOf() => TypedResults.BadRequest(new Foo());

    [HttpGet("conflict-foo")]
    public Conflict<Foo> ReturnConflictOf() => TypedResults.Conflict(new Foo());

    [HttpGet("notfound-foo")]
    public NotFound<Foo> ReturnNotFoundOf() => TypedResults.NotFound(new Foo());

    [HttpGet("unprocessableentity-foo")]
    public UnprocessableEntity<Foo> ReturnUnprocessableEntityOf() => TypedResults.UnprocessableEntity(new Foo());
}
