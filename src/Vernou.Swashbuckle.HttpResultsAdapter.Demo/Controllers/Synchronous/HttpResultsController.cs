namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Controllers.Synchronous;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

public class HttpResultsController : Controller
{
    [HttpGet("ok")]
    public Ok ReturnOk() => TypedResults.Ok();

    [HttpGet("created")]
    public Created ReturnCreated() => TypedResults.Created("");

    [HttpGet("accepted")]
    public Accepted ReturnAccepted() => TypedResults.Accepted("");

    [HttpGet("acceptedatroute")]
    public AcceptedAtRoute ReturnAcceptedAtRoute() => TypedResults.AcceptedAtRoute();

    [HttpGet("nocontent")]
    public NoContent ReturnNoContent() => TypedResults.NoContent();

    [HttpGet("badrequest")]
    public BadRequest ReturnBadRequest() => TypedResults.BadRequest();

    [HttpGet("notfound")]
    public NotFound ReturnNotFound() => TypedResults.NotFound();

    [HttpGet("conflict")]
    public Conflict ReturnChallenge() => TypedResults.Conflict();

    [HttpGet("unauthorized")]
    public UnauthorizedHttpResult ReturnUnauthorized() => TypedResults.Unauthorized();

    [HttpGet("unprocessableentity")]
    public UnprocessableEntity ReturnUnprocessableEntity() => TypedResults.UnprocessableEntity();

    [HttpGet("validationproblem")]
    public ValidationProblem ReturnValidationProblem() => TypedResults.ValidationProblem(new Dictionary<string, string[]>());
}
