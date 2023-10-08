namespace Vernou.Swashbuckle.HttpResultsAdapter.Demo.Controllers;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class HttpResultsController
{
    [HttpGet("ok")]
    public Ok ReturnOk() => TypedResults.Ok();

    [HttpGet("ok-async")]
    public async Task<Ok> ReturnOkAsync() => await Task.FromResult(TypedResults.Ok());

    [HttpGet("created")]
    public Created ReturnCreated() => TypedResults.Created("");

    [HttpGet("created-async")]
    public async Task<Created> ReturnCreatedAsync() => await Task.FromResult(TypedResults.Created(""));

    [HttpGet("accepted")]
    public Accepted ReturnAccepted() => TypedResults.Accepted("");

    [HttpGet("accepted-async")]
    public async Task<Accepted> ReturnAcceptedAsync() => await Task.FromResult(TypedResults.Accepted(""));

    [HttpGet("acceptedatroute")]
    public AcceptedAtRoute ReturnAcceptedAtRoute() => TypedResults.AcceptedAtRoute();

    [HttpGet("acceptedatroute-async")]
    public async Task<AcceptedAtRoute> ReturnAcceptedAtRouteAsync() => await Task.FromResult(TypedResults.AcceptedAtRoute());

    [HttpGet("nocontent")]
    public NoContent ReturnNoContent() => TypedResults.NoContent();

    [HttpGet("nocontent-async")]
    public async Task<NoContent> ReturnNoContentAsync() => await Task.FromResult(TypedResults.NoContent());

    [HttpGet("badrequest")]
    public BadRequest ReturnBadRequest() => TypedResults.BadRequest();

    [HttpGet("badrequest-async")]
    public async Task<BadRequest> ReturnBadRequestAsync() => await Task.FromResult(TypedResults.BadRequest());

    [HttpGet("notfound")]
    public NotFound ReturnNotFound() => TypedResults.NotFound();

    [HttpGet("notfound-async")]
    public async Task<NotFound> ReturnNotFoundAsync() => await Task.FromResult(TypedResults.NotFound());

    [HttpGet("conflict")]
    public Conflict ReturnChallenge() => TypedResults.Conflict();

    [HttpGet("conflict-async")]
    public async Task<Conflict> ReturnChallengeAsync() => await Task.FromResult(TypedResults.Conflict());

    [HttpGet("unauthorized")]
    public UnauthorizedHttpResult ReturnUnauthorized() => TypedResults.Unauthorized();

    [HttpGet("unauthorized-async")]
    public async Task<UnauthorizedHttpResult> ReturnUnauthorizedAsync() => await Task.FromResult(TypedResults.Unauthorized());

    [HttpGet("unprocessableentity")]
    public UnprocessableEntity ReturnUnprocessableEntity() => TypedResults.UnprocessableEntity();

    [HttpGet("unprocessableentity-async")]
    public async Task<UnprocessableEntity> ReturnUnprocessableEntityAsync() => await Task.FromResult(TypedResults.UnprocessableEntity());

    [HttpGet("validationproblem")]
    public ValidationProblem ReturnValidationProblem() => TypedResults.ValidationProblem(new Dictionary<string, string[]>());

    [HttpGet("validationproblem-async")]
    public async Task<ValidationProblem> ReturnValidationProblemAsync() => await Task.FromResult(TypedResults.ValidationProblem(new Dictionary<string, string[]>()));
}
